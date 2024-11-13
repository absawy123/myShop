using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repository;

namespace myShop.DataAccess.Implementation
{
    public class CategoryRepo : BaseRepository<Category>, ICategoryRepo
    {
        private readonly AppDbContext _context;
        public CategoryRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }



    }
}
