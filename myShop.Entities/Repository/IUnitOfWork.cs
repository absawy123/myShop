namespace myShop.Entities.Repository
{
    public interface IUnitOfWork 
    {
        public ICategoryRepo Category { get; }
        public IProductRepo Product { get; }
        public IShoppingCartRepo ShoppingCart { get; }
        public IOrderRepo Order { get; }
        public IOrderDetailRepo OrderDetail { get; }
		public IApplicationUserRepo ApplicationUser { get; }

		int SaveChanges();

    }
}
