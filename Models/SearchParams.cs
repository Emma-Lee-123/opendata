namespace OpenData.Functions.Models
{
    public class SearchParams
    {
        public string from
        {
            get; set;
        }
        public string to
        {
            get; set;
        }
        public string date
        {
            get; set;
        }
        public string startTime
        {
            get; set;
        }
        public string transportType
        {
            get; set;
        } //'All' | 'Bus' | 'Train'
    }
}