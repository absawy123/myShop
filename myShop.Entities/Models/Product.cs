using myShop.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace myShop.Entities.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }


    }
}
