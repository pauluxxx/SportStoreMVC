using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public IEnumerable<CartLine> Lines { get { return lineCollection; } }
        public void AddItem(Product prod, int quantity)
        {
            CartLine line = lineCollection
                .Where(e => e.Product.ProductId == prod.ProductId)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = prod,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void Clear()
        {
            lineCollection.Clear();
        }
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }
        public void RemoveLine(Product prod)
        {
            lineCollection.RemoveAll(e => e.Product.ProductId == prod.ProductId);
        }
        
    }
    public class CartLine
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }

}
