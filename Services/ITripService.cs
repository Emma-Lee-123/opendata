using OpenData.Functions.Models;

namespace OpenData.Functions.Services
{
    public interface ITripService
    {
        //Task<IEnumerable<TripStopGroup>> GetTripStopGroupsAsync(SearchParams searchParams);
        Task<IEnumerable<TripStopGroup>> GetTop5TripStopGroupsAsync(SearchParams searchParams);
    }
}