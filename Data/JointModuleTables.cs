namespace DxBlazorApplication7.Data
{
    public partial class JointModuleTables
    {
        public int Id { get; set; }

        public string? Process { get; set; }

        public string StationNumber { get; set; } = null!;

        public string StationName { get; set; } = null!;

        public bool IsMultipleJobs { get; set; }

        public string? ProcessType { get; set; }

        public bool? IsDelete { get; set; }

        public bool IsFinal { get; set; }
    }
}
