using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.Entities.Models;
using myShop.Entities.Repository;
using System.Security.Claims;

namespace myShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }

        public IActionResult Index()
        {
            return View(_unitOfWork.Product.GetAll(navProperties:"Category").ToList());
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart Obj = new ShoppingCart()
            {
                Product = _unitOfWork.Product
                .GetFirstOrDefault(p => p.Id == productId, navProperties: "Category"),
                Count = 1
            };
            return View(Obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            shoppingCart.ApplicationUserId = id;
            ShoppingCart cartObj = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                s => s.ProductId == shoppingCart.ProductId && s.ApplicationUserId == id 
                );
            if (cartObj == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                cartObj.Count += shoppingCart.Count;
            }
            _unitOfWork.SaveChanges();
            var products = _unitOfWork.Product.GetAll(navProperties: "Category").ToList();
            return View("Index", products);
        }


    }
}
