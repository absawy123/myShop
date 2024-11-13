using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repository;

namespace myShop.DataAccess.Implementation
{
	

	public class OrderDetailRepo : BaseRepository<OrderDetail>, IOrderDetailRepo
	{
		private readonly AppDbContext _context;
		public OrderDetailRepo(AppDbContext context) : base(context)
		{
			_context = context;
		}


	}
}
