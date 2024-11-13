using System.ComponentModel.DataAnnotations;

namespace myShop.Entities.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage="Name is Required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public ICollection<Product>? Products { get; set; }
    }
}
