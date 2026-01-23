using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DxBlazorApplication7.Data
{
    public class TimePeriod
    {
        public int? PeriodType { get; set; }
        public bool? IsDel { get; set; }
        public bool? IsOT { get; set; }
        public string? PeriodName { get; set; }
        public string? EndTime { get; set; }
        public string? Description { get; set; }
        public string? CreateUser { get; set; }
    }
}
