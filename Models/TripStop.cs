namespace OpenData.Functions.Models
{
    public class TripStop
    {
        public string tripId
        {
            get; set;
        }
        public string tripHeadsign
        {
            get; set;
        }
        //directionId: number;
        public int routeType
        {
            get; set;
        }//0:streetcar;1:subway;2:rail;3:bus;4:ferry;5:cable car;6:funicular;7:trolley bus;8:monorail)
        public string stopId
        {
            get;
            set;
        }
        public int stopSequence
        {
            get;
            set;
        }
        public string arrivalTime
        {
            get;
            set;
        }
        public string departureTime
        {
            get;
            set;
        }
        public string stopName
        {
            get;
            set;
        }
        //stopUrl: string;
    }
}