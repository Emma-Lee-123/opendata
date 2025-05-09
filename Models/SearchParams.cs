namespace OpenData.Functions.Models
{
    public class SearchParams
    {
        public string From
        {
            get; set;
        }
        public string To
        {
            get; set;
        }
        public string Date
        {
            get; set;
        }
        public string StartTime
        {
            get; set;
        }
        public string TransportType
        {
            get; set;
        } //'All' | 'Bus' | 'Train'
    }
}