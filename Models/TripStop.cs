namespace OpenData.Functions.Models
{
    public class TripStop
    {
        public string TripId
        {
            get; set;
        }
        public string TripHeadsign
        {
            get; set;
        }
        //directionId: number;
        public int RouteType
        {
            get; set;
        }//0:streetcar;1:subway;2:rail;3:bus;4:ferry;5:cable car;6:funicular;7:trolley bus;8:monorail)
        public string StopId
        {
            get;
            set;
        }
        public int StopSequence
        {
            get;
            set;
        }
        public string ArrivalTime
        {
            get;
            set;
        }
        public string DepartureTime
        {
            get;
            set;
        }
        public string StopName
        {
            get;
            set;
        }
        //stopUrl: string;
    }
}