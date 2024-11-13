using Microsoft.AspNetCore.Http;

namespace myShop.Entities.ViewModels.CategoryVm
{
    public class AddProductVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

    }
}
