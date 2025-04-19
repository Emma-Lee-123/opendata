using System.Collections.Generic;

namespace OpenData.Functions.Models
{
    public class TripStopGroup
    {
        public string Id
        {
            get;
            set;
        }
        public string DepartureTime
        {
            get;
            set;
        }
        public string ArrivalTime
        {
            get;
            set;
        }
        public string TripHeadsign
        {
            get;
            set;
        }
        public List<TripStop> FirstTripStops
        {
            get; set;
        }
        public Transfer Transfer
        {
            get;
            set;
        }
        public List<TripStop> SecondTripStops
        {
            get;
            set;
        }
    }
}