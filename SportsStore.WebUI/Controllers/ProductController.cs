﻿using System;
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

        public ViewResult List(string category, int page=1)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel
                                               {
                                                   Products = _repository.Products
                                                       .Where(p => category == null || p.Category == category)
                                                       .OrderBy(p => p.ProductID)
                                                       .Skip((page - 1) * PageSize)
                                                       .Take(PageSize),
                                                   PagingInfo = new PagingInfo{
                                                       CurrentPage = page,
                                                       ItemsPerPage = PageSize,
                                                       TotalItems =  category== null ?_repository.Products.Count():_repository.Products.Count(e => e.Category ==category)
                                                   },
                                                   CurrentCategory = category
                                               };
            return View(viewModel);
        }
        
    }
}
