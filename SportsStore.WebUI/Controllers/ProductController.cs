using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public int PageSize = 4;
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository productRepository)
        {
            _repository = productRepository;
        }

        public ViewResult List(int page=1)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel
                                               {
                                                   Products = _repository.Products
                                                   .OrderBy(p => p.ProductID)
                                                   .Skip((page - 1) * PageSize)
                                                   .Take(PageSize),
                                                   PagingInfo = new PagingInfo
                                                                    {
                                                                        CurrentPage = page,
                                                                        ItemsPerPage = PageSize,
                                                                        TotalItems = _repository.Products.Count()
                                                                    }
                                               };
            return View(viewModel);
        }

    }
}
