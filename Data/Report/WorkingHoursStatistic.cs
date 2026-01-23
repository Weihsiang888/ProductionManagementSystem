namespace DxBlazorApplication7.Data
{
    public class WorkingHoursStatistic
    {
        public int Index { get; set; }

        public string WorkOrder { get; set; }

        public string SeriaNumber { get; set; }

        public string OPNO { get; set;}

        public DateTime ReceiveTime { get; set; }

        public DateTime ResponseTime { get; set; }

        public int DiffDay { get; set; }

        public int DiffTime { get; set; }
    }
}
