using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IOrderProcessor _orderProcessor;

        public CartController(IProductRepository repo, IOrderProcessor proc)
        {
            _repository = repo;
            _orderProcessor = proc;
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails){
            if(!cart.Lines.Any()) 
            {
                ModelState.AddModelError("", "Sorry, your cart is empty");
            }
            if(ModelState.IsValid)
            {
                _orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            return View(shippingDetails);
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
                            {
                                Cart = cart,
                                ReturnUrl = returnUrl
                            });
        }
        
        public RedirectToRouteResult AddToCart(Cart cart,int productId, string returnUrl)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if(product!=null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new {returnUrl});
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart,int productId, string returnUrl)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if(product!= null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new {returnUrl});
        }

        public ViewResult Summary(Cart cart)
        {
            return View(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }
    }
}
