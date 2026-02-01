using System.ComponentModel.DataAnnotations;

namespace ECommerce_API.Models.Dto
{
    public class OrderDetailsUpdateDTO
    {
        [Required]
        public int OrderDetailId { get; set; }
        [Required]
        [Range(1,5,ErrorMessage ="Must be between 1 and 5 inclusive")]
        public int Rating { get; set; }
    }
}
