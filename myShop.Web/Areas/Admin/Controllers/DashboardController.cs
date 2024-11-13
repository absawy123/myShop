using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.Entities.Repository;
using Utilities;

namespace myShop.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = AppConstants.Admin)]
    public class DashboardController : Controller
    {

        private IUnitOfWork _unitOfWork;
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.Orders = _unitOfWork.Order.GetAll().Count();
            ViewBag.ApprovedOrders = _unitOfWork.Order.GetAll(x => x.OrderStatus == AppConstants.Approve).Count();
            ViewBag.Users = _unitOfWork.ApplicationUser.GetAll().Count();
            ViewBag.Products = _unitOfWork.Product.GetAll().Count();
            return View();
        }
    }
}
