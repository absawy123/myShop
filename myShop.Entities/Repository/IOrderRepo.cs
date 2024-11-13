using myShop.Entities.Models;

namespace myShop.Entities.Repository
{
	public interface IOrderRepo : IBaseRepository<Order>
	{
		void UpdateOrderStatus(int id, string orderStatus, string paymentStatus);

	}
}
