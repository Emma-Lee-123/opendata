using System.Collections.Generic;

namespace OpenData.Functions.Models
{
    public class TripStopGroup
    {
        public string id
        {
            get;
            set;
        }
        public string departureTime
        {
            get;
            set;
        }
        public string arrivalTime
        {
            get;
            set;
        }
        public string tripHeadsign
        {
            get;
            set;
        }
        public int routeType
        {
            get; set;
        }//0:streetcar;1:subway;2:rail;3:bus;4:ferry;5:cable car;6:funicular;7:trolley bus;8:monorail)
        public List<TripStop> firstTripStops
        {
            get; set;
        }
        public Transfer transfer
        {
            get;
            set;
        }
        public List<TripStop> secondTripStops
        {
            get;
            set;
        }
    }
}