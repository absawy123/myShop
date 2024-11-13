using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repository;

namespace myShop.DataAccess.Implementation
{
    public class ProductRepo : BaseRepository<Product> , IProductRepo
    {
        private readonly AppDbContext _context;

        public ProductRepo(AppDbContext context):base(context)
        {
            _context = context;
        }




    }
}
