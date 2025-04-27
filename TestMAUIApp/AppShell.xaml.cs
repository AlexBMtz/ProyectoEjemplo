using TestMAUIApp.Views;

namespace TestMAUIApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(BookView), typeof(BookView));
            SetupMenuBasedOnRole();
        }

        private async void SetupMenuBasedOnRole()
        {
            string? role = await SecureStorage.GetAsync("userRole");

            BookOption.IsVisible = role == "Admin";
        }
    }
}
