namespace dotnet_project.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public int SellerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime SoldAt { get; set; }
        public int ProductTypeId { get; set; }

    }
}
