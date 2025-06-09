namespace AIRMDataManager.Library.Models
{
    public class JirmModelV01
    { 
        public int PageIndex { get; set; } 
        public int PageSize { get; set; } 
        public int Count { get; set; }
        public object Data { get; set; }
        //public Dictionary<string, object> Content { get; set; } (For Free Flow Key-Value Pair Data)
    }
}
