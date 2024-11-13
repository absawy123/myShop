using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace myShop.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }


    }
}
