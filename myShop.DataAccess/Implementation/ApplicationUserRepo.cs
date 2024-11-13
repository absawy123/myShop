using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repository;

namespace myShop.DataAccess.Implementation
{
	public class ApplicationUserRepo : BaseRepository<ApplicationUser> ,IApplicationUserRepo
    {
        readonly AppDbContext _context;
        public ApplicationUserRepo(AppDbContext context):base(context)
        {
            _context = context;
        }

    }
}
