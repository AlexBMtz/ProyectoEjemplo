using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SharedModels.Models;
using SharedModels.Models.DTO.InputDTO;
using SharedModels.Models.DTO.OutputDTO;
using System.Collections.ObjectModel;
using TestMAUIApp.Services;

namespace TestMAUIApp.ViewModels;

/// <summary>
/// ViewModel de la vista detalle de un libro
/// </summary>
public partial class BookViewModel : ObservableObject
{
    private readonly HttpService _httpService;

    [ObservableProperty]
    int _id;

    [ObservableProperty]
    string _name;

    [ObservableProperty]
    string _publishingYear;

    [ObservableProperty]
    private ObservableCollection<AuthorOutputDTO> _authors = [];

    [ObservableProperty]
    private ObservableCollection<Editorial> _editorials = [];

    [ObservableProperty]
    private AuthorOutputDTO? _selectedAuthor;

    [ObservableProperty]
    private Editorial? _selectedEditorial;

    /// <summary>
    /// Constructor con la instancia del servicio HTTP para el envío de peticiones al backend
    /// </summary>
    /// <param name="httpService">Servicio HTTP para envío de peticiones</param>
    public BookViewModel(HttpService httpService)
    {
        _httpService = httpService;
    }

    /// <summary>
    /// Método que carga la información del libro a desplegar
    /// </summary>
    /// <param name="bookDto">Libro cuya información será mostrada en la vista</param>
    /// <returns>Resultado de una operación asíncrona</returns>
    public async Task LoadBookAsync(BookOutputDTO? bookDto = null)
    {
        if (Authors.Count == 0 || Editorials.Count == 0)
        {
            await LoadDropdownData();
        }

        if (bookDto == null)
            return;


        Id = bookDto.Id;
        Name = bookDto.Name;
        PublishingYear = bookDto.PublishingYear;

        SelectedAuthor = Authors.FirstOrDefault(a => a.Id == bookDto.AuthorId);
        SelectedEditorial = Editorials.FirstOrDefault(e => e.Id == bookDto.EditorialId);
    }

    /// <summary>
    /// Método que carga la información de los autores y las editoriales en los comboboxes de la vista
    /// </summary>
    /// <returns>Resultado de una operación asíncrona</returns>
    private async Task LoadDropdownData()
    {
        var authorsList = await _httpService.GetAllAuthors();
        var publishersList = await _httpService.GetAllPublishers();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Authors = [.. authorsList];
            Editorials = [.. publishersList];
        });
    }

    /// <summary>
    /// Método que verifica si la operación es de creación o de actualización
    /// </summary>
    /// <returns>Resultado de una operación asíncrona</returns>
    [RelayCommand]
    public async Task SaveData()
    {
        if (Id.Equals(0))
            await InsertBook();
        else
            await UpdateBook();
    }

    /// <summary>
    /// Método que crea un nuevo libro y envía la petición para guardarlo en BD
    /// </summary>
    /// <returns>Resultado de una operación asíncrona</returns>
    [RelayCommand]
    public async Task InsertBook()
    {
        if (string.IsNullOrEmpty(Name) || SelectedAuthor == null || SelectedEditorial == null)
        {
            await Shell.Current.DisplayAlert("Error", "Faltan campos obligatorios", "OK");
            return;
        }

        var newBook = new BookInputDTO
        {
            Name = Name,
            PublishingYear = PublishingYear,
            AuthorId = SelectedAuthor.Id,
            EditorialId = SelectedEditorial.Id
        };

        await _httpService.CreateBook(newBook);
        await Shell.Current.DisplayAlert("Éxito", "Libro guardado", "OK");
        WeakReferenceMessenger.Default.Send(new RefreshMessage(true));
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// Método que envía la petición para actualizar los datos de un libro en BD
    /// </summary>
    /// <returns>Resultado de una operación asíncrona</returns>
    [RelayCommand]
    public async Task UpdateBook()
    {
        if (string.IsNullOrEmpty(Name) || SelectedAuthor == null || SelectedEditorial == null)
        {
            await Shell.Current.DisplayAlert("Error", "Faltan campos obligatorios", "OK");
            return;
        }

        var newBook = new BookInputDTO
        {
            Id = Id,
            Name = Name,
            PublishingYear = PublishingYear,
            AuthorId = SelectedAuthor.Id,
            EditorialId = SelectedEditorial.Id
        };

        await _httpService.UpdateBook(newBook);
        await Shell.Current.DisplayAlert("Éxito", "Libro guardado", "OK");
        WeakReferenceMessenger.Default.Send(new RefreshMessage(true));
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// Método que envía la petición para eliminar un libro en BD
    /// </summary>
    /// <returns>Resultado de una operación asíncrona</returns>
    [RelayCommand]
    public async Task DeletePart()
    {
        if (Id.Equals(0))
            return;

        bool action = await Shell.Current.DisplayAlert("Advertencia", "Are you sure you want to delete this book?", "Yes", "No");

        if (!action)
            return;

        var bookToDelete = new BookInputDTO
        {
            Id = Id,
            Name = Name,
            PublishingYear = PublishingYear,
            AuthorId = SelectedAuthor.Id,
            EditorialId = SelectedEditorial.Id
        };

        await _httpService.DeleteBook(bookToDelete);
        WeakReferenceMessenger.Default.Send(new RefreshMessage(true));
        await Shell.Current.GoToAsync("..");

    }

    [RelayCommand]
    public async Task DoneEditing()
    {
        await Shell.Current.GoToAsync("..");
    }
}
