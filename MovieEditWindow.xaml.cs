using System.Windows;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace mozirendszer
{
    public partial class MovieEditWindow : Window
    {
        private int _movieId;
        private readonly string? _token;
        private Movie? _currentMovie;

        public MovieEditWindow(int movieId)
        {
            InitializeComponent();
            _movieId = movieId;
            var app = Application.Current as App;
            if (app?.LoggedInUser?.isAdmin != true)
            {
                MessageBox.Show("Nincs adminisztrátori jogosultsága film szerkesztéséhez.", "Hozzáférés megtagadva", MessageBoxButton.OK, MessageBoxImage.Warning);
                SaveChangesButton.IsEnabled = false; // Letiltjuk a gombot
                //this.Close(); // Vagy bezárhatjuk az ablakot
                return;
            }
            _token = app.AdminToken;
            LoadMovieToEdit();
        }

        private async void LoadMovieToEdit()
        {
            var movie = await ApiService.GetMovieById(_movieId);
            if (movie != null)
            {
                _currentMovie = movie;
                TitleTextBox.Text = movie.Title;
                DescriptionTextBox.Text = movie.Description;
                ReleaseDatePicker.SelectedDate = movie.ReleaseDate;
                // Populate other fields
            }
            else
            {
                MessageBox.Show("Hiba a film adatainak betöltésekor szerkesztéshez.");
                this.Close();
            }
        }

        private async void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TitleTextBox.Text) || string.IsNullOrEmpty(DescriptionTextBox.Text) || ReleaseDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Kérjük, töltse ki az összes mezőt.");
                return;
            }

            if (string.IsNullOrEmpty(_token))
            {
                MessageBox.Show("Nincs bejelentkezve vagy érvényes token.");
                return;
            }

            var updatedMovie = new
            {
                id = _movieId,
                title = TitleTextBox.Text,
                description = DescriptionTextBox.Text,
                releaseDate = ReleaseDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd")
                // Include other properties to update
            };

            var (success, message) = await ApiService.UpdateMovie(updatedMovie, _token);
            MessageBox.Show(message);

            if (success)
            {
                this.Close();
                // Optionally refresh the movie details in the MovieDetailWindow
                if (Owner is MovieDetailWindow movieDetailWindow)
                {
                    movieDetailWindow.LoadMovieDetails();
                }
            }
        }
    }
}