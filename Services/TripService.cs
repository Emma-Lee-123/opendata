using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenData.Functions.Models;
//using OpenData.Functions.Services;
using System.Data;

public class TripService : ITripService
{
    private readonly ILogger<TripService> _logger;
    private readonly IConfiguration _config;

    public TripService(ILogger<TripService> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }


    public async Task<IEnumerable<TripStopGroup>> GetTripStopGroupsAsync(SearchParams searchParams)
    {
        _logger.LogInformation($"GetTripStopGroupsAsync from-{searchParams.from}, to-{searchParams.to}, date-{searchParams.date}, time-{searchParams.startTime}");
        var fromStopId = searchParams.from;
        var toStopId = searchParams.to;
        var date = searchParams.date;
        var startTime = searchParams.startTime;

        ValidateParameters(fromStopId, toStopId);

        var dbConnectionString = _config.GetConnectionString("OpenDataDB");
        if (string.IsNullOrEmpty(dbConnectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured");
        }

        var results = new List<TripStopGroup>();
        const int maxResults = 5;

        try
        {
            // Convert HHMM string to HH:MM:SS format for SQL comparison
            var formattedStartTime = FormatTimeString(startTime);

            using var connection = new SqlConnection(dbConnectionString);
            await connection.OpenAsync();

            // Get direct trips first
            var directTrips = await GetDirectTripsAsync(connection, fromStopId, toStopId, date, formattedStartTime, maxResults);
            results.AddRange(directTrips);

            var tripIds = directTrips.Select(t => t.id).ToList();
            var tripStops = await GetStopsBetweenStopsAsync(tripIds, fromStopId, toStopId);

            foreach (var trip in results)
            {
                if (tripStops.TryGetValue(trip.id, out var stops))
                {
                    stops.ForEach(s => s.tripHeadsign = trip.tripHeadsign);
                    trip.firstTripStops.AddRange(stops);
                }
            }
            _logger.LogInformation("Found {Count} trip options", results.Count);
            return results.Take(maxResults);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Database error while fetching trip options");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching trip options");
            throw;
        }
    }
    private string FormatTimeString(string time)
    {
        // Pad with leading zeros if necessary (e.g., "130" -> "0130")
        time = time.PadLeft(4, '0');

        // Insert colon for HH:MM format (e.g., "0130" -> "01:30")
        return $"{time.Substring(0, 2)}:{time.Substring(2, 2)}:00";
    }

    private async Task<List<TripStopGroup>> GetDirectTripsAsync(
        SqlConnection connection, string fromStopId, string toStopId, string date, string startTime, int maxResults)
    {
        const string sql = @"
            SELECT TOP (@MaxResults)
                t.tripId AS Id,
                os.DepartureTime AS DepartureTime,
                ds.ArrivalTime AS ArrivalTime,
                t.TripHeadsign AS FirstTripHeadsign,
                r.RouteType AS FirstRouteType,
                os.StopId AS FirstStopId,
                os.StopSequence AS FirstStopSequence,
                os.DepartureTime AS FirstDepartureTime,
                NULL AS TransferStopName,
                NULL AS TransferArrivalTime,
                NULL AS TransferDepartureTime,
                NULL AS SecondTripHeadsign,
                NULL AS SecondRouteType,
                NULL AS SecondStopId,
                NULL AS SecondStopSequence
            FROM Trips t
            JOIN Routes r ON t.RouteId = r.RouteId
            JOIN CalendarDates cd ON t.ServiceId = cd.ServiceId
            JOIN StopTimes os ON t.TripId = os.TripId
            JOIN StopTimes ds ON t.TripId = ds.TripId
            WHERE cd.Date = @Date
              AND os.StopId = @FromStopId
              AND ds.StopId = @ToStopId
              AND os.DepartureTime >= @StartTime
              AND ds.StopSequence > os.StopSequence
            ORDER BY os.DepartureTime";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Date", date);
        command.Parameters.AddWithValue("@FromStopId", fromStopId);
        command.Parameters.AddWithValue("@ToStopId", toStopId);
        command.Parameters.AddWithValue("@StartTime", startTime);
        command.Parameters.AddWithValue("@MaxResults", maxResults);

        return await ReadTripStopGroupsAsync(command);
    }
    private async Task<List<TripStopGroup>> ReadTripStopGroupsAsync(SqlCommand command)
    {
        var results = new List<TripStopGroup>();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new TripStopGroup
            {
                id = reader.GetString("Id"),
                tripHeadsign = reader.GetString("FirstTripHeadsign"),
                departureTime = reader.GetString("DepartureTime"),
                arrivalTime = reader.GetString("ArrivalTime"),
                routeType = reader.GetInt32("FirstRouteType"),
                firstTripStops = new List<TripStop>(),
                transfer = new Transfer(),
                secondTripStops = new List<TripStop>()
            });
        }

        return results;
    }

    public async Task<Dictionary<string, List<TripStop>>> GetStopsBetweenStopsAsync(
        List<string> tripIds,
        string startStopId,
        string endStopId)
    {
        if (tripIds == null || !tripIds.Any())
        {
            throw new ArgumentException("At least one trip ID must be provided", nameof(tripIds));
        }

        if (string.IsNullOrEmpty(startStopId))
        {
            throw new ArgumentException("Start stop ID is required", nameof(startStopId));
        }

        if (string.IsNullOrEmpty(endStopId))
        {
            throw new ArgumentException("End stop ID is required", nameof(endStopId));
        }

        var dbConnectionString = _config.GetConnectionString("OpenDataDB");
        var stops = new List<TripStop>();

        try
        {
            using var connection = new SqlConnection(dbConnectionString);
            await connection.OpenAsync();

            // Create comma-separated list of quoted trip IDs for SQL IN clause
            var tripIdList = string.Join(",", tripIds.Select(id => $"'{id}'"));

            var sql = $@"
            SELECT 
                st.TripId,
                st.StopId,
                s.StopName,
                st.ArrivalTime,
                st.DepartureTime,
                st.StopSequence
            FROM StopTimes st
            JOIN Stops s ON st.StopId = s.StopId
            WHERE st.TripId IN ({tripIdList})
            AND st.StopSequence >= (
                SELECT StopSequence 
                FROM StopTimes 
                WHERE TripId = st.TripId AND StopId = @StartStopId
            )
            AND st.StopSequence <= (
                SELECT StopSequence 
                FROM StopTimes 
                WHERE TripId = st.TripId AND StopId = @EndStopId
            )
            ORDER BY st.TripId, st.StopSequence";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@StartStopId", startStopId);
            command.Parameters.AddWithValue("@EndStopId", endStopId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                stops.Add(new TripStop
                {
                    tripId = reader.GetString("TripId"),
                    stopId = reader.GetString("StopId"),
                    stopName = reader.GetString("StopName"),
                    arrivalTime = reader.GetString("ArrivalTime"),
                    departureTime = reader.GetString("DepartureTime"),
                    stopSequence = reader.GetInt32("StopSequence")
                });
            }
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Database error while fetching stops between {StartStopId} and {EndStopId}",
                startStopId, endStopId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching stops between {StartStopId} and {EndStopId}",
                startStopId, endStopId);
            throw;
        }
        var tripStopGroups = stops.GroupBy(s => s.tripId).ToDictionary(s => s.Key, s => s.ToList());
        return tripStopGroups;
    }

    private void ValidateParameters(string fromStopId, string toStopId)
    {
        if (string.IsNullOrEmpty(fromStopId))
        {
            throw new ArgumentException("From stop ID is required", nameof(fromStopId));
        }

        if (string.IsNullOrEmpty(toStopId))
        {
            throw new ArgumentException("To stop ID is required", nameof(toStopId));
        }

        if (fromStopId == toStopId)
        {
            throw new ArgumentException("From and to stop IDs must be different");
        }
    }

}