using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly GlobalViewModel _global;
        private readonly IClientService _clientService; 

        [ObservableProperty]
        private string email = "user3@mail.com";

        [ObservableProperty]
        private string password = "user3";

        [ObservableProperty]
        private string loginMessage;

        // IClientService via DI meegeven
        public LoginViewModel(IAuthService authService, GlobalViewModel global, IClientService clientService)
        {
            _authService = authService;
            _global = global;
            _clientService = clientService;
        }

        [RelayCommand]
        private void Login()
        {
            Client? authenticatedClient = _authService.Login(Email, Password);
            if (authenticatedClient != null)
            {
                LoginMessage = $"Welkom {authenticatedClient.Name}!";

                _global.Client = authenticatedClient;
                _clientService.CurrentClient = authenticatedClient; // 👈 direct instellen

                Application.Current.MainPage = new AppShell();
            }
            else
            {
                LoginMessage = "Ongeldige inloggegevens.";
            }
        }
    }
}
