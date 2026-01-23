namespace DxBlazorApplication7.Data
{
    public class WorkingOrderStepList
    {
        public DateTime? WorkingTime { get; set; }
        public int? ID { get; set; }
        public int? WOID { get; set; }
        public string? StationName { get; set; }
        public string? WorkOrder { get; set; }
        public string? SerialNumber { get; set; }
        public string? OPNO { get; set; }
        public string? WorkingStatus { get; set; }
        public DateTime? StatusTime { get; set; }
        public string? ReasonType { get; set; }
        public string? CreateUser { get; set; }
    }
}
