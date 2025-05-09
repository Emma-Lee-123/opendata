using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using OpenData.Functions.Models;

namespace OpenData.Functions
{
    public class TripFunction
    {
        private readonly ILogger<TripFunction> _logger;
        private readonly ITripService _tripService;
        public TripFunction(ILogger<TripFunction> logger, ITripService tripService)
        {
            _tripService = tripService;
            _logger = logger;
        }

        [Function("get-trip-stops")]
        // public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        // {
        //     _logger.LogInformation("C# HTTP trigger function processed a request.");
        //     return new OkObjectResult("Welcome to Azure Functions!");
        // }
        // [Function("get-stop-suggestions")]
        public async Task<HttpResponseData> GetTripStopsAsyn([HttpTrigger(AuthorizationLevel.Function, "get", Route = "trips/search")] HttpRequestData req
, FunctionContext context)
        {
            _logger.LogInformation("get-trip-stops function processed a request.");
            var queryParams = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var fromStop = queryParams["from"];
            var toStop = queryParams["to"];
            var date = queryParams["date"];
            var startTime = queryParams["startTime"];
            var TransportType = queryParams["transportType"];
            var searchParams = new SearchParams
            {
                From = string.IsNullOrEmpty(fromStop) ? string.Empty : fromStop,
                To = string.IsNullOrEmpty(toStop) ? string.Empty : toStop,
                Date = string.IsNullOrEmpty(date) ? string.Empty : date,
                StartTime = string.IsNullOrEmpty(startTime) ? string.Empty : startTime,
                TransportType = string.IsNullOrEmpty(TransportType) ? string.Empty : TransportType
            };
            _logger.LogInformation($"get-trip-stops function processed a request. from-{fromStop}, to-{toStop}, date-{date}, time-{startTime}");
            //todo: validate input parameters
            if (fromStop == "" || toStop == "" || date == "" || startTime == string.Empty)
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                //badResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await badResponse.WriteStringAsync("Invalid parameters");
                return badResponse;
            }
            var trips = await _tripService.GetTripStopGroupsAsync(searchParams);
            var response = req.CreateResponse(HttpStatusCode.OK);
            var json = JsonSerializer.Serialize(trips, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            response.Headers.Remove("Content-Type");
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(json);
            return response;

        }
    }
}
