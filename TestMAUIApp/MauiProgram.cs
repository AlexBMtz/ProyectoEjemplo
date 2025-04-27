using Microsoft.Extensions.Logging;
using TestMAUIApp.Services;
using TestMAUIApp.ViewModels;
using TestMAUIApp.Views;

namespace TestMAUIApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Montserrat-Regular.ttf", "Montserrat");
                }).Services.AddHttpClient<HttpService>(client =>
            {
                client.BaseAddress = new Uri("http://192.168.100.7:5000/api/");
            }).Services
            .AddTransient<LoginViewModel>()
            .AddTransient<BooksViewModel>()
            .AddTransient<BookViewModel>()
            .AddTransient<LoginView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
