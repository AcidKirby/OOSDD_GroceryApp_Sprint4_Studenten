using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;

        public BoughtProductsService(
            IGroceryListItemsRepository groceryListItemsRepository,
            IGroceryListRepository groceryListRepository,
            IClientRepository clientRepository,
            IProductRepository productRepository)
        {
            _groceryListItemsRepository = groceryListItemsRepository;
            _groceryListRepository = groceryListRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
        }

        public List<BoughtProducts> Get(int? productId)
        {
            var result = new List<BoughtProducts>();

            if (productId == null)
                return result;

            // Haal alle boodschappenlijsten op
            var groceryLists = _groceryListRepository.GetAll();

            foreach (var list in groceryLists)
            {
                // Haal de items van deze lijst
                var items = _groceryListItemsRepository.GetAllOnGroceryListId(list.Id);

                foreach (var item in items)
                {
                    // Check of dit item overeenkomt met het gevraagde product
                    if (item.ProductId == productId.Value)
                    {
                        var client = _clientRepository.Get(list.ClientId);
                        var product = _productRepository.Get(productId.Value);

                        if (client != null && product != null)
                        {
                            // Voeg de combinatie client + lijst + product toe
                            result.Add(new BoughtProducts(client, list, product));
                        }
                    }
                }
            }

            return result;
        }
    }
}
