using SharedModels.Models.DTO.OutputDTO;
using TestMAUIApp.ViewModels;

namespace TestMAUIApp.Views;

[QueryProperty("BookToDisplay", "book")]
public partial class BookView : ContentPage
{
    private readonly BookViewModel _viewModel;
    private BookOutputDTO _bookToDisplay;

    public BookView(BookViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await _viewModel.LoadBookAsync();
        });
    }
    public BookOutputDTO BookToDisplay
    {
        get => _bookToDisplay;
        set
        {
            //Se valida si un nuevo libro ha sido seleccionado para ser desplegado
            if (_bookToDisplay == value)
                return;
            _bookToDisplay = value;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _viewModel.LoadBookAsync(_bookToDisplay);
            });
        }
    }
}