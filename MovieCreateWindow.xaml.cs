using System.Windows;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace mozirendszer
{
    public partial class MovieCreateWindow : Window
    {
        private readonly string? _token;

        public MovieCreateWindow()
        {
            InitializeComponent();
            var app = Application.Current as App;
            if (app?.LoggedInUser?.isAdmin != true)
            {
                MessageBox.Show("Nincs adminisztrátori jogosultsága film létrehozásához.", "Hozzáférés megtagadva", MessageBoxButton.OK, MessageBoxImage.Warning);
                CreateMovieButton.IsEnabled = false; // Letiltjuk a gombot
                //this.Close(); // Vagy bezárhatjuk az ablakot
                return;
            }
            _token = app.AdminToken;
        }

        private async void CreateMovieButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TitleTextBox.Text) || string.IsNullOrEmpty(DescriptionTextBox.Text) || string.IsNullOrEmpty(ReleaseDate.Text))
            {
                MessageBox.Show("Kérjük, töltse ki az összes mezőt.");
                return;
            }

            if (string.IsNullOrEmpty(_token))
            {
                MessageBox.Show("Nincs bejelentkezve vagy érvényes token.");
                return;
            }

            var newMovie = new
            {
                title = TitleTextBox.Text,
                description = DescriptionTextBox.Text,
                releaseDate = ReleaseDate.Text
                // Add other properties as needed
            };

            var (success, message) = await ApiService.CreateMovie(newMovie, _token);
            MessageBox.Show(message);

            if (success)
            {
                this.Close();
                // Optionally refresh the movie list in the MovieListWindow
                if (Owner is MovieListWindow movieListWindow)
                {
                    movieListWindow.LoadMovies();
                }
            }
        }
    }
}