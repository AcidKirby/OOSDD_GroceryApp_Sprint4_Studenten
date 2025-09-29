
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
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        public List<BoughtProducts> Get(int? productId)
{
    var result = new List<BoughtProducts>();

    if (productId == null)
        return result;

    // haal alle boodschappenlijsten op
    var groceryLists = _groceryListRepository.GetAll();

    foreach (var list in groceryLists)
    {
        // haal de items van deze lijst
        var items = _groceryListItemsRepository.GetAllOnGroceryListId(list.Id);

        // check of dit product in de lijst zit
        if (items.Any(i => i.ProductId == productId))
        {
            var client = _clientRepository.Get(list.ClientId);
            var product = _productRepository.Get(productId.Value);

            if (client != null && product != null)
            {
                result.Add(new BoughtProducts(client, list, product));
            }
        }
    }

    return result;
}

    }
}
