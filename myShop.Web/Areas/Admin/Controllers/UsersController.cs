using Microsoft.AspNetCore.Mvc;
using myShop.DataAccess.Data;
using myShop.Entities.ViewModels.UserVm;
using System.Security.Claims;

namespace myShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var users = _context.ApplicationUsers
                .Where(user => user.Id != id)
                .Select(u => 
                new UserInfoVm(){
                    userId =u.Id,
                    Name = u.Name,
                    Address =u.Address,
                    Email=u.Email,
                  LockoutEnd = u.LockoutEnd  
                })
                .ToList();
            return View(users);
        }

        public IActionResult LockUnlock(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(user== null)
            {
                return NotFound();
            }
            if(user.LockoutEnd > DateTime.Now )
            {
                user.LockoutEnd = DateTime.Now;
                _context.SaveChanges();
                return Json(new { status = "unlocked" });
            }
            else{
                user.LockoutEnd = DateTime.Now.AddHours(5);
                _context.SaveChanges();
                return Json(new { status = "locked" });
            }
        }


    }
}
