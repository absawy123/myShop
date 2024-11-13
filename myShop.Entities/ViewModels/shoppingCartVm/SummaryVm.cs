using myShop.Entities.Models;

namespace myShop.Entities.ViewModels.shoppingCartVm
{
	public class SummaryVm
	{
		public List<ShoppingCart> CartList { get; set; }
        public Order Order { get; set; }
    }
}
