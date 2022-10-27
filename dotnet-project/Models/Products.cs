using System.ComponentModel.DataAnnotations;

namespace dotnet_project.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int SellerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime SoldAt { get; set; }
        public int ProductTypeId { get; set; }

    }
}
