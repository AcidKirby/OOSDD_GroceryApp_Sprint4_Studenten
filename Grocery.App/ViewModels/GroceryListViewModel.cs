using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListViewModel : BaseViewModel
    {
        public ObservableCollection<GroceryList> GroceryLists { get; set; }
        private readonly IGroceryListService _groceryListService;
        private readonly IClientService _clientService;

        public Client CurrentClient => _clientService.CurrentClient; // 👈 haalt ingelogde gebruiker op

        public GroceryListViewModel(IGroceryListService groceryListService, IClientService clientService)
        {
            Title = "Boodschappenlijst";
            _groceryListService = groceryListService;
            _clientService = clientService;
            GroceryLists = new(_groceryListService.GetAll());
        }

        [RelayCommand]
        public async Task SelectGroceryList(GroceryList groceryList)
        {
            Dictionary<string, object> parameter = new()
            {
                { nameof(GroceryList), groceryList }
            };

            await Shell.Current.GoToAsync(
                $"{nameof(GroceryListItemsView)}?Titel={groceryList.Name}",
                true,
                parameter
            );
        }

        [RelayCommand]
        public async Task ShowBoughtProducts()
        {
            if (CurrentClient != null && CurrentClient.Role == Role.Admin)
            {
             
                await Shell.Current.GoToAsync(nameof(BoughtProductsView));
            }
            // geen actie voor niet-admins
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            GroceryLists = new(_groceryListService.GetAll());
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            GroceryLists.Clear();
        }
    }
}
