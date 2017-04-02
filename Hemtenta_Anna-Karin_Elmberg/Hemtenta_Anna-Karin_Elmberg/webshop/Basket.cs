using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017
{
    public class Basket : IBasket
    {
        List<Product> products = new List<Product>();

        public decimal TotalCost
        {
            get;
            private set;
        }

        public void AddProduct(Product p, int amount)
        {
            if (p == null)
            {
                throw new ArgumentNullException();
            }

            if (amount < 1)
            {
                throw new IllegalAmountException();
            }

            for (int i = 0; i < amount; i++)
            {
                products.Add(p);
                TotalCost += p.Price;
            }
        }

        public void RemoveProduct(Product p, int amount)
        {
            if (p == null)
            {
                throw new ArgumentNullException();
            }

            if (amount <= 0)
            {
                throw new IllegalAmountException();
            }

            for (int i = 0; i < amount; i++)
            {
                products.Remove(p);
                TotalCost -= p.Price;
            }
        }
    }
}
