using System.Collections.Generic;
using System.Linq;
using ProductAPI.Data;

namespace ProductAPI.Repository
{
    public interface IProductRepository
    {
        Product AddProduct(Product product);
        void DeleteProduct(int id);
        void DeleteProductByName(string name);
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        Product UpdateProduct(Product product);
        int GetProductsCount();
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public Product AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return product;
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public void DeleteProductByName(string name)
        {
            var product = _context.Products.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public int GetProductsCount()
        {
            return _context.Products.ToList().Count();
        }
    }
}