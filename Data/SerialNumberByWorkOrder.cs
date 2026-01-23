using Microsoft.EntityFrameworkCore;

namespace DxBlazorApplication7.Data
{
    public class SerialNumberByWorkOrder
    {
        public string? OrderNo { get; set; }
        public string? SerialNO { get; set; }
        public string? LineName { get; set; }
        public string? PP_Name { get; set; }
        public string? PartNo { get; set; }
        public string? PartName { get; set; }
        public int? Quantity { get; set; }
        public string? StartTime { get; set; }
    }
}
