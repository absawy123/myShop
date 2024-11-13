using myShop.Entities.Models;

namespace myShop.Entities.ViewModels.shoppingCartVm
{
    public class ShoppingCartVm
    {
        public List<ShoppingCart> CartList { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
