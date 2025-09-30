using Xunit;
using Grocery.Core.Models;
using Grocery.Core.Services;          // voor ProductService en BoughtProductsService
using Grocery.App.ViewModels;         // voor BoughtProductsViewModel
using System.Linq;

namespace TestsUC13
{
    public class BoughtProductsViewModelTests
    {
        [Fact]
        public void NewSelectedProduct_ShouldUpdateBoughtProductsList()
        {
            // Arrange
            var productService = new ProductService();               // concrete service
            var boughtProductsService = new BoughtProductsService(); // concrete service

            // Voeg testdata toe (hangt af van implementatie van jouw services!)
            var testProduct = new Product(1, "Test Product", 10);
            productService.Add(testProduct);

            var boughtProduct = new BoughtProducts { ProductId = 1, Buyer = "Alice", Quantity = 2 };
            boughtProductsService.Add(boughtProduct);

            var viewModel = new BoughtProductsViewModel(boughtProductsService, productService);

            // Act
            viewModel.NewSelectedProduct(testProduct);

            // Assert
            Assert.Single(viewModel.BoughtProductsList);
            Assert.Equal("Alice", viewModel.BoughtProductsList.First().Buyer);
            Assert.Equal(2, viewModel.BoughtProductsList.First().Quantity);
        }
    }
}