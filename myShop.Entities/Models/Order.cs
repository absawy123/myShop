using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace myShop.Entities.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime PaymentDate { get; set; }
        //  stripe properties
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        // data of user
        public string UserName { get; set; }
        public string? UserAddress { get; set; }
        public string? UserPhoneNumber { get; set; }
        public string City { get; set; }


    }
}
