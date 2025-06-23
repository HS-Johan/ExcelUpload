namespace ExcelUpload.ViewModel
{
    public class ProductVM
    {
        public Guid ProductGuidId { get; set; }

        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? Sku { get; set; }

        public string? ProductCategory { get; set; }
        public string? SupplierId { get; set; }
        public int Stock { get; set; }

        public bool IsPublished { get; set; }
        public DateTime ActiveStartDate { get; set; }
        
        public double SellPrice { get; set; }
        public double BuyPrice { get; set; }

        public double EmployeeCost { get; set; }
        public double OtherCost { get; set; }
    }
}
