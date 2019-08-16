using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
                                                    {
                                                        new Product{ProductID = 1, Name = "P1"},
                                                        new Product{ProductID = 2, Name = "P2"},
                                                        new Product{ProductID = 3, Name = "P3"},
                                                        new Product{ProductID = 4, Name = "P4"},
                                                        new Product{ProductID = 5, Name = "P5"},
                                                    }.AsQueryable());
            ProductController controller = new ProductController(mock.Object)
                                               {
                                                   PageSize = 3
                                               };
            ProductsListViewModel resutl = (ProductsListViewModel) controller.List(null, 2).Model;

            Product[] prodArray = resutl.Products.ToArray();
            Assert.IsTrue(prodArray.Length==2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name,"P5");

        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
                                        {
                                            CurrentPage = 2,
                                            TotalItems = 28,
                                            ItemsPerPage = 10
                                        };

            Func<int, string> pageUrlDelegate = i => "Page" + i;
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(result.ToString(),@"<a href=""Page1"">1</a><a class=""selected"" href=""Page2"">2</a><a href=""Page3"">3</a>");
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                                                        new Product{ ProductID = 1, Name = "P1"},
                                                        new Product{ ProductID = 2, Name = "P2"}, 
                                                        new Product{ ProductID = 3, Name = "P3"}, 
                                                        new Product{ ProductID = 4, Name = "P4"}, 
                                                        new Product{ ProductID = 5, Name = "P5"}
 
                                                    }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel) controller.List(null, 2).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);

        }

        [TestMethod]
        public void Can_Filter_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ ProductID=1, Name = "P1", Category = "Cat1"},
                new Product{ ProductID=2, Name = "P2", Category = "Cat2"},
                new Product{ ProductID=3, Name = "P3", Category = "Cat1"},
                new Product{ ProductID=4, Name = "P4", Category = "Cat2"},
                new Product{ ProductID=5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());
            ProductController controller = new ProductController(mock.Object)
                                               {
                                                   PageSize = 3
                                               };

            Product[]result = ((ProductsListViewModel) controller.List("Cat2").Model).Products.ToArray();
            
            Assert.AreEqual(result.Length,2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }
    }
}
