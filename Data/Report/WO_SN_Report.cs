namespace DxBlazorApplication7.Data
{
    public class WO_SN_Report
    {
        public string WO { get; set; }

        public string SN { get; set; }

        public DateTime FirstReceiveTime { get; set; }

        public DateTime LastResponseTime { get; set; }

        public string WorkTime { get; set; }

        public string MaxWorkTime { get; set; }

        public string MinWorkTime { get; set; }

        public string AvgWorkTime { get; set; }

        public string OPGroup { get; set; }

        public int WorkCount { get; set; }
    }
}
