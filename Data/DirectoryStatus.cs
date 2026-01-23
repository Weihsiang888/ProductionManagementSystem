namespace DxBlazorApplication7.Data
{
    public class DirectoryStatus
    {
        public string DirectoryName { get; set; }
        public string DirectoryMax { get; set; }
        public string FileName { get; set; }
        public List<string> Directorys { get; set; }
        public bool? IsSelect { get; set; }
    }
}
