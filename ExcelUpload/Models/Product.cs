using System.ComponentModel.DataAnnotations;

namespace ExcelUpload.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public Guid ProductGuidId { get; set; }

        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? Sku { get; set; }
        public string? ProductCategory { get; set; }
        public int SupplierId { get; set; }
        public Guid SupplierGuidId { get; set; }
        public int Stock { get; set; }
        public string? Image { get; set; }
        public bool IsPublished { get; set; }
        public DateTime ActiveStartDate { get; set; }
        public DateTime? ExpiryDate { get; set; } 
        public double SellPrice { get; set; }
        public double BuyPrice { get; set; }
        public double EmployeeCost { get; set; }
        public double OtherCost { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
