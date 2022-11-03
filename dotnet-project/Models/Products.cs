using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace dotnet_project.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please provide a name for your product")]
        public string? ProductName { get; set; }
        [Required(ErrorMessage = "Please provide a description for this product")]
        public string? ProductDescription { get; set; }
        [Required(ErrorMessage = "Please provide us with the amount of the product you have")]
        [Min(1, ErrorMessage = "You must add atleast one product")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Please provide how much you want to sell this product for")]
        public int Price { get; set; }
        [Required]
        public int SellerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime SoldAt { get; set; }
        public int ProductTypeId { get; set; }

    }
}
