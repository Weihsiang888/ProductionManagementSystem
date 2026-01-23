using Microsoft.EntityFrameworkCore;

namespace DxBlazorApplication7.Data
{
    public class WorkingOrderList
    {
        public DateTime? WorkingTime { get; set; }
        public int? ID { get; set; }
        public string? StationName { get; set; }
        public string? WorkOrder { get; set; }
        public string? SerialNumber { get; set; }
        public double? WorkOrderCount { get; set; }
        public string? OPNO { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? ReceiveTime { get; set; }
        public bool? IsReceive { get; set; }
        public DateTime? PauseTime { get; set; }
        public bool? IsPause { get; set; }
        public DateTime? ResponseTime { get; set; }
        public bool? IsResponse { get; set; }
        public string? CreateUser { get; set; }
        public double? PassCount { get;set; }
        public double? FailCount { get; set; }
        public string? ProcessType { get; set; }
    }
}
