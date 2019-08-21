using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _repository;

        public AdminController(IProductRepository repo)
        {
            _repository = repo;
        }

        public ViewResult Index()
        {
            return View(_repository.Products);
        }

        public ViewResult Edit(int productId)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if(ModelState.IsValid)
            {
                _repository.SaveProject(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product prod = _repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if(prod != null)
            {
                _repository.DeleteProduct(prod);
                TempData["message"] = string.Format("{0} was deleted", prod.Name);
            }
            return RedirectToAction("Index");
        }
    }
}