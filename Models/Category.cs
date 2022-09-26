using System.ComponentModel.DataAnnotations;

namespace QRMenuManagementSystem.Models
{
    public class Category
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool CategoryActive { get; set; }
    }
}
