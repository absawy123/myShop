using Microsoft.AspNetCore.Mvc;
using myShop.Entities.Models;
using myShop.Entities.Repository;

namespace myShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(_unitOfWork.Category.GetAll());
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.SaveChanges();
                TempData["Create"] = "Category created successfully";
                return RedirectToAction("Index", new {area="Admin"});
            }

            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // edit first or default in base repo
            var category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                return View(category);
            }
            return Json("category not found");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.SaveChanges();
                TempData["Update"] = "Category updated successfully";
                return RedirectToAction("Index", new {area="Admin"});
            }
            return View("Edit", category);
        }
        public IActionResult Delete(int id)
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _unitOfWork.Category.Remove(category);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index", new {area="Admin"});
            }
            return Json("Can not delete category");
        }

    }
}
