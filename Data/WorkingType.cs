using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DxBlazorApplication7.Data
{
    public class WorkingType
    {
        public string? TypeID { get; set; }
        public string? TypeName { get; set; }
        public string? TypeGroup { get; set; }
        public bool? IsDel { get; set; }
        public string? CreateUser { get; set; }
    }
}
