using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SharedModels.Models.DTO.OutputDTO;
using System.Collections.ObjectModel;
using TestMAUIApp.Services;
using TestMAUIApp.Views;

namespace TestMAUIApp.ViewModels;

/// <summary>
/// ViewModel de la vista general de los libros
/// </summary>
public partial class BooksViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<BookOutputDTO> _books;

    [ObservableProperty]
    private bool _isRefreshing = false;


    [ObservableProperty]
    private bool _isBusy = false;

    [ObservableProperty]
    private BookOutputDTO? _selectedBook;

    private readonly HttpService _httpService;

    /// <summary>
    /// Constructor con la instancia del servicio HTTP para el envío de peticiones al backend
    /// </summary>
    /// <param name="httpService">Servicio HTTP para envío de peticiones</param>
    public BooksViewModel(HttpService httpService)
    {
        _httpService = httpService;
        _books = [];

        WeakReferenceMessenger.Default.Register<RefreshMessage>(this, async (r, m) =>
        {
            await LoadData();
        });

        //Nos permite realizar peticiones asíncronas sin utilizar la palabra async o await en el método
        //Se realiza de esta manera porque el constructor de una clase nunca puede ser asíncrono
        Task.Run(LoadData);
    }

    /// <summary>
    /// Método invocado cuando se selecciona un libro de la lista desplegada en la vista
    /// </summary>
    /// <returns>Resultado de una operación asíncrona</returns>
    [RelayCommand]
    public async Task BookSelected()
    {
        if (SelectedBook == null)
            return;

        var navigationParameter = new Dictionary<string, object>()
    {
        { "book", SelectedBook }
    };

        await Shell.Current.GoToAsync(nameof(BookView), navigationParameter);

        MainThread.BeginInvokeOnMainThread(() => SelectedBook = null);
    }

    /// <summary>
    /// Método que carga la vista con los datos recuperados de la tabla de libros de la BD
    /// </summary>
    /// <returns>Resultado de una operación asíncrona</returns>
    [RelayCommand]
    public async Task LoadData()
    {
        if (IsBusy)
            return;

        try
        {
            IsRefreshing = true;
            IsBusy = true;

            var booksCollection = await _httpService.GetBooks();

            //Invoca una acción en el hilo principal de la aplicación
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Books.Clear();

                foreach (BookOutputDTO book in booksCollection)
                {
                    Books.Add(book);
                }
            });
        }
        finally
        {
            IsRefreshing = false;
            IsBusy = false;
        }
    }

    /// <summary>
    /// Método que llama a la vista de detalle para agregar un nuevo libro
    /// </summary>
    /// <returns>Resultado de una operación asíncrona</returns>
    [RelayCommand]
    public static async Task AddNewBook()
    {
        await Shell.Current.GoToAsync(nameof(BookView));
    }
}
