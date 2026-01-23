using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DxBlazorApplication7.Data
{
    public partial class OperatorInformation
    {
        public string? Department { get; set; }
        public string? OperatorName { get; set; }
        public string? OperatorEnName { get; set; }
        public string? Mail { get; set; }
        public string? Phone { get; set; }
        public string? OPNO { get; set; }
        public string? CARDNO { get; set; }
        public bool? IsDel { get; set; }
        public bool? IsManager { get; set; }
        public bool? IsSPDGroup { get; set; }
        public string? Ability { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? CreateUser { get; set;}
    }
}
