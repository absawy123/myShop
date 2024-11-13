using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.Entities.Models;
using myShop.Entities.Repository;
using myShop.Entities.ViewModels.shoppingCartVm;
using Stripe.Checkout;
using System.Security.Claims;
using Utilities;

namespace myShop.Web.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private ShoppingCartVm cartVm { get; set; }
		public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			cartVm = new ShoppingCartVm()
			{
				CartList = _unitOfWork.ShoppingCart
			   .GetAll(s => s.ApplicationUserId == userId, navProperties: "Product").ToList()
			};
			foreach (var item in cartVm.CartList)
			{
				cartVm.TotalPrice += (item.Count * item.Product.Price);
			}
			return View(cartVm);
		}

		public IActionResult Plus(int cartId)
		{
			var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(s => s.Id == cartId, navProperties: "Product");
			shoppingCart.Count += 1;
			_unitOfWork.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Minus(int cartId)
		{
			var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(s => s.Id == cartId, navProperties: "Product");
			if (shoppingCart.Count <= 1)
			{
				_unitOfWork.ShoppingCart.Remove(shoppingCart);
				_unitOfWork.SaveChanges();
				return RedirectToAction("Index", "Home");
			}
			else
			{
				shoppingCart.Count -= 1;
			}

			_unitOfWork.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Remove(int cartId)
		{
			var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(s => s.Id == cartId, navProperties: "Product");
			_unitOfWork.ShoppingCart.Remove(shoppingCart);
			_unitOfWork.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Summary()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			var summaryVm = new SummaryVm()
			{
				CartList=_unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == userId ,navProperties:"Product").ToList(),
				Order = new()
			};

			summaryVm.Order.ApplicationUser =  _unitOfWork.ApplicationUser.GetFirstOrDefault(a => a.Id == userId);
			summaryVm.Order.UserName = summaryVm.Order.ApplicationUser.Name;
			summaryVm.Order.UserAddress = summaryVm.Order.ApplicationUser.Address;
			summaryVm.Order.UserPhoneNumber = summaryVm.Order.ApplicationUser.PhoneNumber;
			summaryVm.Order.City = summaryVm.Order.ApplicationUser.City;
			foreach(var item in summaryVm.CartList)
			{
				summaryVm.Order.TotalPrice += (item.Count * item.Product.Price);
			}

			return View(summaryVm);

		}

		[HttpPost]
		public IActionResult Summary(SummaryVm summaryVm)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			summaryVm.CartList =_unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == userId,navProperties:"Product").ToList();

			summaryVm.Order.OrderStatus = AppConstants.Pending;
			summaryVm.Order.PaymentStatus = AppConstants.Pending;
			summaryVm.Order.OrderDate = DateTime.Now;
			summaryVm.Order.ApplicationUserId = userId;
			summaryVm.Order.PaymentIntentId =Guid.NewGuid().ToString();
			summaryVm.Order.SessionId = Guid.NewGuid().ToString();

			foreach (var item in summaryVm.CartList)
			{
				summaryVm.Order.TotalPrice += (item.Count * item.Product.Price);
			}
			_unitOfWork.Order.Add(summaryVm.Order);
			_unitOfWork.SaveChanges();
			foreach(var item in summaryVm.CartList)
			{
				OrderDetail orderDetail = new OrderDetail()
				{
					OrderId = summaryVm.Order.Id,
					ProductId = item.ProductId,
					Price = item.Product.Price,
					Count = item.Count,
				};
				_unitOfWork.OrderDetail.Add(orderDetail);
				_unitOfWork.SaveChanges();
			}

			var domain = "http://localhost:5034/";
			var options = new SessionCreateOptions
			{
				LineItems = new List<SessionLineItemOptions>(),

				Mode = "payment",
				SuccessUrl = domain + $"customer/cart/orderconfirmation?id={summaryVm.Order.Id}",
				CancelUrl = domain + $"customer/cart/index",
			};

			foreach (var item in summaryVm.CartList)
			{
				var sessionlineoption = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.Product.Price * 100),
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Product.Name,
						},
					},
					Quantity = item.Count,
				};
				options.LineItems.Add(sessionlineoption);
			}


			var service = new SessionService();
			Session session = service.Create(options);
			summaryVm.Order.SessionId = session.Id;

			_unitOfWork.SaveChanges();

			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);

			//_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.CartsList);
			//         _unitOfWork.Complete();
			//         return RedirectToAction("Index","Home");


		}

		public IActionResult OrderConfirmation(int id)
		{
			Order orderHeader = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == id);
			var service = new SessionService();
			Session session = service.Get(orderHeader.SessionId);

			if (session.PaymentStatus.ToLower() == "paid")
			{
				orderHeader.OrderStatus = AppConstants.Approve;
				orderHeader.PaymentIntentId = session.PaymentIntentId;
				_unitOfWork.SaveChanges();
			}
			List<ShoppingCart> shoppingcarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
			//HttpContext.Session.Clear();
			_unitOfWork.ShoppingCart.RemoveRange(shoppingcarts);
			_unitOfWork.SaveChanges();
			return View(id);
		}

        private int GetCartItemCount()
        {
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			int cartCount = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == userId).Count();
            return cartCount;
        }

        [HttpGet]
        public JsonResult CartItemCount() // recieve ajax call to send shopping cart count
        {
            int count = GetCartItemCount();
            return Json(new { count });
        }

    }
}
