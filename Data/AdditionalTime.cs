namespace DxBlazorApplication7.Data
{
    public class AdditionalTime
    {
        public DateTime? AddTimeDate { get; set; }
        public string? AddTimeType { get; set; }
        public string? Description { get; set; }
        public string? OPNO { get; set; }
        public DateTime AddStartTime { get; set; }
        public DateTime AddEndTime { get; set; }
        public double AddTime { get; set; }
        public string? ReasonType { get; set; }
        public bool? IsApprove { get; set; }
        public bool? IsDel { get; set; }
        public string? CreateUser { get; set; }
    }
}
