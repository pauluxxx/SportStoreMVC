using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Concrete
{
    public  class EFProductRespository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Product> Products
        {
            get { return context.Products; }
        }


        public void SaveProduct(Product product)
        {
            if (product.ProductId==0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product prod = context.Products.Find(product.ProductId);
                if (prod!=null)
                {
                    prod.Name = product.Name;
                    prod.Description = product.Description;
                    prod.Price = product.Price;
                    prod.Category = product.Category;
                    prod.ImageData = product.ImageData;
                    prod.ImageMimeType = product.ImageMimeType;
                }
            }

            context.SaveChanges();
        }


        public Product DeleteProduct(int productId)
        {
            Product prodEntry = context.Products.Find(productId);
            if (prodEntry!=null)
            {
                context.Products.Remove(prodEntry);
                context.SaveChanges();
            }
            return prodEntry;
        }
    }
}
