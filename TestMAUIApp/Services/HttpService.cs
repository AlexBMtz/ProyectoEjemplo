using SharedModels.Models;
using SharedModels.Models.DTO.InputDTO;
using SharedModels.Models.DTO.OutputDTO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace TestMAUIApp.Services;

/// <summary>
/// Clase que permite el envío de peticiones HTTP al Web API
/// </summary>
public class HttpService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions;
    private string? _authorizationKey;

    public HttpService(HttpClient client)
    {
        _client = client;
        _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<bool> InitializeClient(string email, string password)
    {
        try
        {
            if (string.IsNullOrEmpty(_authorizationKey))
            {
                var response = await Login(email, password);
                if (!response.IsSuccessStatusCode)
                    return false;

                var contentStream = await response.Content.ReadAsStreamAsync();
                var token = await JsonSerializer.DeserializeAsync<UserTokenOutputDTO>(contentStream, _serializerOptions);
                _authorizationKey = token?.AccessToken;

                if (!string.IsNullOrEmpty(_authorizationKey))
                {
                    await SecureStorage.SetAsync("accessToken", _authorizationKey);

                    var user = await GetCurrentUser();
                    if (user != null)
                    {
                        await SecureStorage.SetAsync("userName", $"{user.FirstName} {user.LastName}");
                        await SecureStorage.SetAsync("userRole", user.Role);
                    }
                }
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en InitializeClient: {ex.Message}");
            return false;
        }
    }


    public async Task<HttpResponseMessage> Login(string username, string password)
    {
        var loginDto = new LoginInputDTO
        {
            Email = username,
            Password = password
        };

        return await _client.PostAsJsonAsync("Auth/login", loginDto, _serializerOptions);
    }

    public async Task<LoggedInUserOutputDTO?> GetCurrentUser()
    {
        try
        {
            var response = await _client.GetAsync("User");
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var user = await JsonSerializer.DeserializeAsync<LoggedInUserOutputDTO>(stream, _serializerOptions);

            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GetCurrentUser: {ex.Message}");
            return null;
        }
    }

    public async Task<List<BookOutputDTO>> GetBooks()
    {
        var response = await _client.GetAsync("Book");
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var books = await JsonSerializer.DeserializeAsync<List<BookOutputDTO>>(stream, _serializerOptions);
        return books ?? [];
    }

    public async Task<List<AuthorOutputDTO>> GetAllAuthors()
    {
        try
        {
            var response = await _client.GetAsync("Author");
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var authors = await JsonSerializer.DeserializeAsync<List<AuthorOutputDTO>>(stream, _serializerOptions);
            return authors ?? [];
        }
        catch { return []; }
    }

    public async Task<List<Editorial>> GetAllPublishers()
    {
        try
        {
            var response = await _client.GetAsync("Editorial");
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var editorials = await JsonSerializer.DeserializeAsync<List<Editorial>>(stream, _serializerOptions);
            return editorials ?? [];
        }
        catch { return []; }
    }

    public async Task<HttpResponseMessage> CreateBook(BookInputDTO newBook) => await _client.PostAsJsonAsync("Book", newBook);

    public async Task<HttpResponseMessage> UpdateBook(BookInputDTO updatedBook) => await _client.PutAsJsonAsync($"Book/{updatedBook.Id}", updatedBook);

    public async Task DeleteBook(BookInputDTO bookToDelete)
    {
        await _client.DeleteAsync($"Book/{bookToDelete.Id}");
    }
}
