using Microsoft.AspNetCore.Mvc;
using myShop.Entities.Models;
using myShop.Entities.Repository;
using myShop.Entities.ViewModels.CategoryVm;

namespace myShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProducts()  //recieve Ajax call , return data to show in DataTable
        {
            var products = _unitOfWork.Product.GetAll().Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Description,
                CategoryName = p.Category.Name
            })
                .ToList();
            return Json(new { data = products });
        }

        [HttpGet]
        public IActionResult Add()
        {
            var categoryList = _unitOfWork.Category.GetAll().Select(c => new { c.Name, c.Id });
            ViewBag.CategoryList = categoryList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddProductVm productVm,IFormFile file)
        {

            if (ModelState.IsValid)
            {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);
                    string fullPath = wwwrootPath + @"\Images\products\" + fileName + extension;
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    Product product = new()
                    {
                        Name = productVm.Name,
                        Id = productVm.Id,
                        Price = productVm.Price,
                        Description = productVm.Description,
                        ImgPath = $"Images/Products/{fileName}{extension}",
                        CategoryId = productVm.CategoryId,
                        Category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == productVm.CategoryId)
                    };
                _unitOfWork.Product.Add(product);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index",new {area="Admin"});
            }

            return View(productVm);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                var categories =_unitOfWork.Category.GetAll().Select(c => new { c.Id, c.Name }).ToList();
                ViewBag.CategoryList = categories;
                return View(product);
            }
            return Json("Product not found");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if(file != null)
                {
                 string wwwrootPath = _webHostEnvironment.WebRootPath;
                 string oldFilePath = wwwrootPath+ "\\Images\\Products\\"+Path.GetFileName(product.ImgPath);

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);

                    }

                    string newFileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);
                    string fullPath = wwwrootPath + @"\Images\products\" + newFileName + extension;
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    product.ImgPath = $"Images/Products/{newFileName}{extension}";
                }
                _unitOfWork.Product.Update(product);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index",new {area="Admin"});
            }
            var categories = _unitOfWork.Category.GetAll().Select(c => new { c.Id, c.Name }).ToList();
            ViewBag.CategoryList = categories;
            return View("Edit", product);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);
            if (product != null)
            {
                string WebRootPath =_webHostEnvironment.WebRootPath;
                string ImageName =Path.GetFileName(product.ImgPath);
                string oldImagePath = WebRootPath + "\\Images\\Products\\" + ImageName;
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.Product.Remove(product);
                _unitOfWork.SaveChanges();
                return Json(new {success =true,message ="product has been deleted"});
            }
            return Json(new { success = false, message = "couldnt delete product" });
        }
    }
}
