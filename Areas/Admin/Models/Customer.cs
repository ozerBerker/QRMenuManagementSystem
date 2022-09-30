using System.ComponentModel.DataAnnotations;

namespace QRMenuManagementSystem.Areas.Admin.Models
{
    public class Customer
    {
        [Key]
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerQrCode { get; set; }
        public string CustomerDatabaseSecret { get; set; }
        public string CustomerBasePath { get; set; }
        public string CustomerAuthApiKey { get; set; }
        public bool CustomerActive { get; set; }
    }
}
