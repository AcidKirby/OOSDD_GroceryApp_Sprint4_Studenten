using NUnit.Framework;
using Grocery.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace TestCore
{
    public class BoughtProductsServiceTests
    {
        private List<Product> _products;
        private List<Client> _clients;
        private List<GroceryList> _lists;
        private List<GroceryListItem> _items;

        [SetUp]
        public void Setup()
        {
            // Arrange testdata
            _products = new List<Product>
            {
                new Product(1, "Milk", 10)
            };

            _clients = new List<Client>
            {
                new Client(1, "Alice", "alice@mail.com", "pw1"),                // default Role.None
                new Client(2, "Bob", "bob@mail.com", "pw2"),                    // default Role.None
                new Client(3, "user3", "admin@mail.com", "pw3") { Role = Role.Admin }  // admin
            };

            _lists = new List<GroceryList>
            {
                new GroceryList(1, "ListAlice", DateOnly.FromDateTime(System.DateTime.Today), "red", _clients[0].Id),
                new GroceryList(2, "ListBob",   DateOnly.FromDateTime(System.DateTime.Today), "blue", _clients[1].Id),
                new GroceryList(3, "ListAdmin", DateOnly.FromDateTime(System.DateTime.Today), "green", _clients[2].Id)
            };

            _items = new List<GroceryListItem>
            {
                new GroceryListItem(1, _lists[0].Id, _products[0].Id, 2), // Alice bought Milk
                new GroceryListItem(2, _lists[2].Id, _products[0].Id, 1)  // Admin bought Milk
                // Bob heeft niks gekocht
            };
        }

        [Test]
        public void GetClientsWhoBoughtProduct_ShouldReturnAliceAndAdmin()
        {
            // Act: bepaal alle clients die Milk hebben gekocht
            var buyers = (from list in _lists
                          join item in _items on list.Id equals item.GroceryListId
                          join client in _clients on list.ClientId equals client.Id
                          where item.ProductId == 1
                          select client).Distinct().ToList();

            // Assert
            Assert.That(buyers.Count, Is.EqualTo(2));
            Assert.That(buyers.Any(c => c.Name == "Alice"), Is.True);
            Assert.That(buyers.Any(c => c.Name == "user3"), Is.True); // admin
        }

        [Test]
        public void GetClientsWhoBoughtProduct_ShouldNotReturnBob()
        {
            // Act
            var buyers = (from list in _lists
                          join item in _items on list.Id equals item.GroceryListId
                          join client in _clients on list.ClientId equals client.Id
                          where item.ProductId == 1
                          select client).Distinct().ToList();

            // Assert
            Assert.That(buyers.Any(c => c.Name == "Bob"), Is.False);
        }
        [Test]
        public void GetClientsWhoBoughtProduct_NoPurchases_ShouldReturnEmptyList()
        {
            // Arrange: maak een product dat niemand heeft gekocht
            var newProduct = new Product(2, "Bread", 5);

            // Act
            var buyers = (from list in _lists
                          join item in _items on list.Id equals item.GroceryListId
                          join client in _clients on list.ClientId equals client.Id
                          where item.ProductId == newProduct.Id
                          select client).Distinct().ToList();

            // Assert
            Assert.That(buyers, Is.Empty);
        }
    }

}

