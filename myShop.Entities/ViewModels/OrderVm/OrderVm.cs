using myShop.Entities.Models;

namespace myShop.Entities.ViewModels.OrderVm
{
	public class OrderVm
	{
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}
