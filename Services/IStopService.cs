public interface IStopService
{
    Task<IEnumerable<StopSuggestion>> GetStopSuggestionsAsync(string pattern, int maxResults);
    // Task<IEnumerable<TripStopGroup>> GetTripStopGroupsAsync(SearchParams searchParams);
}