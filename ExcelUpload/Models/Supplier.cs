using System.ComponentModel.DataAnnotations;

namespace ExcelUpload.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        public Guid SupplierGuidId { get; set; }
        public string? SupplierName { get; set; }
        public string? Address { get; set; }

        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
