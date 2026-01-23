using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DxBlazorApplication7.Data
{
    public class ComparisonTable
    {
        public int ID { get; set; }
        public string ProcessName { get; set; }
        public string StationName { get; set; }
    }
}
