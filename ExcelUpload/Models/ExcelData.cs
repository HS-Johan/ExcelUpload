using System.ComponentModel.DataAnnotations;

namespace ExcelUpload.Models
{
    public class ExcelData
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Age { get; set; }
        public string? City { get; set; }
    }

}
