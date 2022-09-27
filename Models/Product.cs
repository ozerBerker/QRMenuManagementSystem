namespace QRMenuManagementSystem.Models
{
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public Category category { get; set; }
        public string CategoryId { get; set; }

    }
}
