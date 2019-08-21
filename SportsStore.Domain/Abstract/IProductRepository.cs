using System.Linq;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Abstract
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        
        void SaveProject(Product product);
        void DeleteProduct(Product product);
    }
}