using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace mozirendszer
{
    public static class ApiService
    {
        private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:4444/api/") };

        public static async Task<(bool success, string message, User user, string token)> Login(string email, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { emailAddress = email, password }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("users/loginCheck", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return (false, responseBody, null, null);

            var json = JsonDocument.Parse(responseBody).RootElement;
            var user = JsonSerializer.Deserialize<User>(json.GetProperty("user").ToString());
            var token = json.GetProperty("token").GetString();
            return (true, "Sikeres bejelentkezés!", user, token);
        }

        public static async Task<(bool success, string message)> Register(string username, string email, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { username, emailAddress = email, password }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("users/register", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            return (response.IsSuccessStatusCode, responseBody);
        }

        public static async Task<User> GetUserById(int id, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"users/getUser/{id}");
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(responseBody);
        }
        public static async Task<(bool success, string message)> CreateMovie(object movieData, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonSerializer.Serialize(movieData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("movies/movies", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, responseBody);
        }

        public static async Task<ObservableCollection<Movie>> GetAllMovies()
        {
            var response = await client.GetAsync("movies/movies");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ObservableCollection<Movie>>(responseBody);
            }
            return null;
        }

        public static async Task<Movie> GetMovieById(int movieId)
        {
            var response = await client.GetAsync($"movies/movies/{movieId}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Movie>(responseBody);
            }
            return null;
        }

        public static async Task<(bool success, string message)> UpdateMovie(object movieData, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var movieIdProperty = movieData.GetType().GetProperty("id");
            if (movieIdProperty == null)
            {
                return (false, "A film azonosítója hiányzik a frissítendő adatokból.");
            }
            var movieId = movieIdProperty.GetValue(movieData);
            var content = new StringContent(JsonSerializer.Serialize(movieData), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"movies/movies/{movieId}", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, responseBody);
        }

        public static async Task<(bool success, string message)> DeleteMovie(int movieId, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync($"movies/movies/{movieId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, responseBody);
        }

        public static async Task<ObservableCollection<Movie>> SearchMoviesByTitle(string title)
        {
            var response = await client.GetAsync($"movies/movies/{title}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ObservableCollection<Movie>>(responseBody);
            }
            return null;
        }
    }
}
