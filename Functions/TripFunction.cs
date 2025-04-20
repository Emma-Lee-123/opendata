using System.Net;
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
                From = fromStop,
                To = toStop,
                Date = date,
                StartTime = startTime,
                TransportType = TransportType
            };

            //todo: validate input parameters

            var trips = await _tripService.GetTripStopGroupsAsync(searchParams);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(trips);
            return response;

        }
    }
}
