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
        public int RouteType
        {
            get; set;
        }//0:streetcar;1:subway;2:rail;3:bus;4:ferry;5:cable car;6:funicular;7:trolley bus;8:monorail)
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