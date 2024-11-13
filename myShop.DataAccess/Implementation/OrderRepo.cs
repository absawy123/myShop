using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repository;

namespace myShop.DataAccess.Implementation
{
	public class OrderRepo : BaseRepository<Order> , IOrderRepo
    {
        private readonly AppDbContext _context;
        public OrderRepo(AppDbContext context):base(context) 
        {
            _context = context;
        }

		public void UpdateOrderStatus(int id, string orderStatus, string paymentStatus)
		{
			var order = _context.Orders.FirstOrDefault(o => o.Id == id);
			if (order != null)
			{
				order.OrderStatus = orderStatus;
				if(paymentStatus != null)
					order.PaymentStatus = paymentStatus;
			}
		}
		

	}
}
