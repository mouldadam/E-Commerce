using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce_API.Models.Dto
{
    public class OrderHeaderCreateDTO
    {
        [Required]
        public string PickUpName { get; set; } = string.Empty;
        [Required]
        public string PickUpPhoneNumber { get; set; } = string.Empty;
        [Required]
        public string PickUpEmail { get; set; } = string.Empty;
        [Required]
        public string ApplicationUserid { get; set; } = string.Empty;
        public double OrderTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalItem { get; set; }
        public List<OrderDetailsCreateDTO> OrderDetails { get; set; } = new();
    }
}
