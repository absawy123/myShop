using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.Entities.Repository;
using myShop.Entities.ViewModels.OrderVm;
using Stripe;
using Utilities;

namespace myShop.Web.Areas.Admin.Controllers
{
    [Area(AppConstants.Admin)]
	[Authorize(Roles =AppConstants.Admin)]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		[BindProperty]
		public OrderVm OrderVM { get; set; }
		public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index() => View();

        [HttpGet]
		public IActionResult GetOrders()
		{
            var orders = _unitOfWork.Order.GetAll(navProperties: "ApplicationUser").ToList();
            return Json(new { data = orders });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            OrderVm orderVm = new OrderVm()
            {
                Order = _unitOfWork.Order.GetFirstOrDefault(o => o.Id == id, navProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetail.GetAll(od => od.OrderId == id,navProperties:"Product").ToList()
            };
            return View(orderVm);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateOrderDetails()
		{

			var orderfromdb = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == OrderVM.Order.Id);
			orderfromdb.UserName = OrderVM.Order.UserName;
			orderfromdb.UserPhoneNumber = OrderVM.Order.UserPhoneNumber;
			orderfromdb.UserAddress = OrderVM.Order.UserAddress;
			orderfromdb.City = OrderVM.Order.City;

			if (OrderVM.Order.Carrier != null)
			{
				orderfromdb.Carrier = OrderVM.Order.Carrier;
			}

			if (OrderVM.Order.TrackingNumber != null)
			{
				orderfromdb.TrackingNumber = OrderVM.Order.TrackingNumber;
			}

			_unitOfWork.Order.Update(orderfromdb);
			_unitOfWork.SaveChanges();
			//TempData["Update"] = "Item has Updated Successfully";
			return RedirectToAction("Details", "Order", new { area = "Admin", id = orderfromdb.Id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartProccess()
		{
			var orderfromdb = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == OrderVM.Order.Id);
			orderfromdb.OrderStatus = AppConstants.Proccessing;
			_unitOfWork.SaveChanges();
			//TempData["Update"] = "order has Updated Successfully";
			return RedirectToAction("Details", "Order", new { area = "Admin", id = OrderVM.Order.Id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartShip()
		{
			var orderfromdb = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == OrderVM.Order.Id);
			orderfromdb.Carrier = OrderVM.Order.Carrier;
			orderfromdb.TrackingNumber = OrderVM.Order.TrackingNumber;
			orderfromdb.ShippingDate = DateTime.Now;
			orderfromdb.OrderStatus = AppConstants.Shipped;

			_unitOfWork.Order.Update(orderfromdb);
			_unitOfWork.SaveChanges();
			//TempData["Update"] = "Item has Shipped Successfully";
			return RedirectToAction("Details", new { area = "Admin", id = orderfromdb.Id });
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var orderfromdb = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == OrderVM.Order.Id);
            if (orderfromdb.PaymentStatus == AppConstants.Approve)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderfromdb.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(option);
				orderfromdb.OrderStatus =AppConstants.Refund;
            }
            else
            {
                orderfromdb.OrderStatus = AppConstants.Cancelled;
            }
            _unitOfWork.SaveChanges();

            //TempData["Update"] = "Order has Cancelled Successfully";
            return RedirectToAction("Details", new { area = "Admin", id = orderfromdb.Id });
        }


    }
}
