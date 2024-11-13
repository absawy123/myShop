using AutoMapper;
using myShop.DataAccess.Data;
using myShop.Entities.Repository;

namespace myShop.DataAccess.Implementation
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryRepo Category { get; private set; }
        public IProductRepo Product { get; private set; }
        public IShoppingCartRepo ShoppingCart { get; private set; }
        public IOrderRepo Order { get;private set; }
        public IOrderDetailRepo OrderDetail { get; private set; }
		public IApplicationUserRepo ApplicationUser { get; private set; }


		public UnitOfWork(AppDbContext context  )
        {
            _context = context;
            Category = new CategoryRepo(_context);
            Product = new ProductRepo(_context);
            ShoppingCart = new ShoppingCartRepo(_context);
            Order = new OrderRepo(_context);
            OrderDetail = new OrderDetailRepo(_context);
            ApplicationUser = new ApplicationUserRepo(_context);
        }

        public int SaveChanges() => _context.SaveChanges();
        

    }
}
