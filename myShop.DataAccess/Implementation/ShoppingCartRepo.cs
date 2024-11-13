using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repository;

namespace myShop.DataAccess.Implementation
{
    public class ShoppingCartRepo : BaseRepository<ShoppingCart>, IShoppingCartRepo
    {
        private readonly AppDbContext _context;

        public ShoppingCartRepo(AppDbContext context):base(context)
        {
            _context = context;
        }


    }
}
