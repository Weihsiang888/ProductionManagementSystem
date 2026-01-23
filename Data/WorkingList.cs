using Microsoft.EntityFrameworkCore;

namespace DxBlazorApplication7.Data
{
    public class WorkingList
    {
        public DateTime? WorkingTime { get; set; }
        public int? WorkingID { get; set; }
        public string? WorkingPeriod { get; set; }
        public string? WorkingName { get; set; }
        public string? WorkingDescription { get; set; }
        public string? ProcessName { get; set; }
        public string? ProcessDescription { get; set; }
        public string? WorkOrder { get; set; }
        public string? PartNo { get; set; }
        public string? PartName { get; set; }
        public string? SeriaNumber { get; set; }
        public double? WorkOrderCount { get; set; }
        public string? OPNO { get; set; }
        public string? OPName { get; set; }
        public bool? IsDel { get; set; }
        public DateTime? AssignTime { get; set; }
        public bool? IsAssign { get; set; }
        public DateTime? PauseTime { get; set; }
        public bool? IsPause { get; set; }
        public DateTime? ReceiveTime { get; set; }
        public bool? IsReceive { get; set; }
        public DateTime? ResponseTime { get; set; }
        public bool? IsResponse { get; set; }
        public string? CreateUser { get; set; }
    }
}
