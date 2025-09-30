using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Models;
using System;

namespace Grocery.App.Resources.ViewModels
{
    public partial class GroceryListViewModel : ObservableObject // of BaseViewModel als die bestaat
    {
        [ObservableProperty]
        private Client client;

        // Hook voor unittests
        public Action OnNavigateBoughtProducts { get; set; } = () => { };

        public GroceryListViewModel()
        {
            // Initieer de Client property zodat de compiler geen warnings geeft
            Client = new Client(0, "Unknown", "unknown@mail.com", "pw");
        }

        [RelayCommand]
        public void ShowBoughtProducts()
        {
            if (Client.Role == Role.Admin)
            {
                // in de echte app: navigeren naar BoughtProductsView
                // bv. Shell.Current.GoToAsync(nameof(BoughtProductsView));

                // in tests: zet de hook
                OnNavigateBoughtProducts();
            }
        }
    }
}
