using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DxBlazorApplication7.Data
{
    public class WorkingTypeGroup
    {
        public string? TypeGroup { get; set; }
        public string? TypeGroupName { get; set; }
        public string? ManufacturingProcess { get; set; }
        public bool? IsDel { get; set; }
        public string? CreateUser { get; set; }
    }
}
