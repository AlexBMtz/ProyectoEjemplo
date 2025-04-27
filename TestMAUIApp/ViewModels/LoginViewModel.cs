using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TestMAUIApp.Services;

namespace TestMAUIApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly HttpService _httpService;
        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _isRunning;

        public LoginViewModel(HttpService httpService)
        {
            _httpService = httpService;
        }

        [RelayCommand]
        public async Task Login()
        {
            IsRunning = true;

            if (Email == null || Password == null)
            {
                await Application.Current.Windows[0].Page.DisplayAlert("Error", "Campos obligatorios faltantes", "Ok");
                return;
            }


            bool result = await _httpService.InitializeClient(Email, Password);

            if (result)
            {
                Application.Current.Windows[0].Page = new AppShell();
                return;
            }

            await Application.Current.Windows[0].Page.DisplayAlert("Error", "Inicio de sesión fallido", "Ok");
            IsRunning = false;

        }
    }
}

