using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class StopService : IStopService
{
    private readonly ILogger<StopService> _logger;
    private readonly IConfiguration _config;

    public StopService(ILogger<StopService> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    public async Task<IEnumerable<StopSuggestion>> GetStopSuggestionsAsync(string pattern, int maxResults)
    {
        _logger.LogInformation("Fetching stop suggestions for pattern: {Pattern} with max results: {MaxResults}", pattern, maxResults);
        if (string.IsNullOrEmpty(pattern))
        {
            throw new ArgumentException("Pattern is required", nameof(pattern));
        }

        if (maxResults < 1 || maxResults > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(maxResults), "Max results must be between 1 and 100");
        }
        var suggestions = new List<StopSuggestion>();
        var dbConnectionString = _config.GetConnectionString("OpenDataDB");
        _logger.LogInformation("Database connection string: {DbConnectionString}", dbConnectionString);
        if (string.IsNullOrEmpty(dbConnectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured");
        }
        using var connection = new SqlConnection(dbConnectionString);
        await connection.OpenAsync();
        var sql = "SELECT TOP (@MaxResults) StopId, StopName FROM Stops WHERE StopName LIKE @Pattern ORDER BY StopName";
        _logger.LogInformation("SQL Query: {Sql}", sql);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Pattern", $"%{pattern}%");
        command.Parameters.AddWithValue("@MaxResults", maxResults);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var suggestion = new StopSuggestion
            {
                StopId = reader.GetString(0),
                StopName = reader.GetString(1)
            };
            suggestions.Add(suggestion);
        }
        _logger.LogInformation("Fetched {Count} stop suggestions", suggestions.Count);
        return suggestions;
    }
}