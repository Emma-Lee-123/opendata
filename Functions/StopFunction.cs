using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace OpenData.Functions
{
    public class StopFunction
    {
        private readonly ILogger<StopFunction> _logger;
        private readonly IStopService _stopService;
        public StopFunction(IStopService stopService, ILogger<StopFunction> logger)
        {
            _stopService = stopService;
            _logger = logger;
        }

        [Function("get-stop-suggestions")]
        public async Task<HttpResponseData> GetStopSuggestionsAsyn([HttpTrigger(AuthorizationLevel.Function, "get", Route = "stops/suggest")] HttpRequestData req
        , FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var queryParams = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var pattern = queryParams["pattern"];
            var maxResults = queryParams["maxResults"];
            if (string.IsNullOrEmpty(maxResults) || !int.TryParse(maxResults, out int maxResultsValue) || maxResultsValue < 1 || maxResultsValue > 100)
            {
                maxResultsValue = 10; // Default value
            }

            if (string.IsNullOrEmpty(pattern))
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync("{\"error\": \"Pattern is required\"}");
                return response;
            }
            else
            {
                var suggestions = await _stopService.GetStopSuggestionsAsync(pattern, maxResultsValue);
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(suggestions);
                return response;
            }
        }
    }
}
