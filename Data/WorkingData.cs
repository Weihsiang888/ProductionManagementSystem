using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DxBlazorApplication7.Data
{
    public class WorkingData
    {
        public DateTime? WorkingDate { get; set; }
        public string? WorkingType { get; set; }
        public string? WorkingID { get; set; }
        public string? WorkingDescription { get; set; }
        public string? WorkingGroup { get; set; }
        public string? OPNO { get; set; }
        public int? QTY { get; set; }
        public bool? IsDel { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? CreateUser { get; set; }
    }
}
