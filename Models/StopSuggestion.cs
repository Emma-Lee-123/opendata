public class StopSuggestion
{
    public string StopId
    {
        get; set;
    }
    public string StopName
    {
        get; set;
    }
}

// public class SearchParams
// {
//     public string From
//     {
//         get; set;
//     }
//     public string To
//     {
//         get; set;
//     }
//     public string Date
//     {
//         get; set;
//     }
//     public string StartTime
//     {
//         get; set;
//     }
//     public string TransportType
//     {
//         get; set;
//     } //'All' | 'Bus' | 'Train'
// }
// public class Transfer
// {
//     public string StopName
//     {
//         get;
//         set;
//     }
//     public string ArrivalTime
//     {
//         get;
//         set;
//     }
//     public string DepartureTime
//     {
//         get; set;
//     }
// }
// public class TripStop
// {
//     public string TripHeadsign
//     {
//         get; set;
//     }
//     //directionId: number;
//     public int RouteType
//     {
//         get; set;
//     }//0:streetcar;1:subway;2:rail;3:bus;4:ferry;5:cable car;6:funicular;7:trolley bus;8:monorail)
//     public string StopId
//     {
//         get;
//         set;
//     }
//     public int StopSequence
//     {
//         get;
//         set;
//     }
//     public string ArrivalTime
//     {
//         get;
//         set;
//     }
//     public string DepartureTime
//     {
//         get;
//         set;
//     }
//     public string StopName
//     {
//         get;
//         set;
//     }
//     //stopUrl: string;
// }
// public class TripStopGroup
// {
//     public string Id
//     {
//         get;
//         set;
//     }
//     public string DepartureTime
//     {
//         get;
//         set;
//     }
//     public string ArrivalTime
//     {
//         get;
//         set;
//     }
//     public List<TripStop> FirstTripStops
//     {
//         get; set;
//     }
//     public Transfer Transfer
//     {
//         get;
//         set;
//     }
//     public List<TripStop> SecondTripStops
//     {
//         get;
//         set;
//     }
// }