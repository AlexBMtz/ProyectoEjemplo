using TestMAUIApp.Views;

namespace TestMAUIApp
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; }
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            // Guarda el ServiceProvider si quieres
            ServiceProvider = serviceProvider;

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Carga el LoginPage desde el contenedor
            NavigationPage loginView = new(ServiceProvider.GetRequiredService<LoginView>());

            return new Window(loginView);
        }

    }
}