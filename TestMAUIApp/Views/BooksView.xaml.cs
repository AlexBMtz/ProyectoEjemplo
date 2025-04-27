using TestMAUIApp.ViewModels;

namespace TestMAUIApp.Views;

public partial class BooksView : ContentPage
{
    public BooksView(BooksViewModel booksViewModel)
    {
        InitializeComponent();
        BindingContext = booksViewModel;
    }
}