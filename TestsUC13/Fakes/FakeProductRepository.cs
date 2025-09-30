using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace UC13Tests.Fakes
{
    public class FakeProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();

        public List<Product> GetAll() => _products;

        public Product? Get(int id) => _products.FirstOrDefault(p => p.Id == id);

        public Product Add(Product item)
        {
            _products.Add(item);
            return item;
        }

        public Product? Delete(Product item)
        {
            var existing = _products.FirstOrDefault(p => p.Id == item.Id);
            if (existing != null)
            {
                _products.Remove(existing);
                return existing;
            }
            return null;
        }

        public Product? Update(Product item)
        {
            var existing = _products.FirstOrDefault(p => p.Id == item.Id);
            if (existing != null) _products.Remove(existing);
            _products.Add(item);
            return item;
        }
    }
}
