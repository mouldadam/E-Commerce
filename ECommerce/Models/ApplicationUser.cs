using Microsoft.AspNetCore.Identity;

namespace ECommerce_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
