using System.Windows;

namespace mozirendszer
{
    public partial class MovieDetailWindow : Window
    {
        private int _movieId;

        // Példa a MovieDetailWindow.xaml.cs konstruktorában
        public MovieDetailWindow(int movieId)
        {
            InitializeComponent();
            _movieId = movieId;
            LoadMovieDetails();

            // Admin gombok láthatóságának beállítása
            var app = Application.Current as App;
            if (app?.LoggedInUser?.isAdmin == true)
            {
                EditMovieButton.Visibility = Visibility.Visible;
                DeleteMovieButton.Visibility = Visibility.Visible;
            }
            else
            {
                EditMovieButton.Visibility = Visibility.Collapsed;
                DeleteMovieButton.Visibility = Visibility.Collapsed;
            }
        }

        public async void LoadMovieDetails()
        {
            var movie = await ApiService.GetMovieById(_movieId);
            if (movie != null)
            {
                TitleLabel.Content = movie.Title;
                DescriptionTextBlock.Text = movie.Description;
                ReleaseDateLabel.Content = movie.ReleaseDate.ToShortDateString();
                // Display other details as needed
            }
            else
            {
                MessageBox.Show("Hiba a film részleteinek betöltésekor.");
                this.Close();
            }
        }

        private void EditMovieButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            if (app?.LoggedInUser?.isAdmin == true)
            {
                var editWindow = new MovieEditWindow(_movieId);
                editWindow.Owner = this; // Beállítjuk az owner-t az esetleges frissítéshez
                editWindow.ShowDialog(); // Dialogként nyitjuk meg
                LoadMovieDetails(); // Frissítjük a részleteket a szerkesztés után
            }
            else
            {
                MessageBox.Show("Nincs adminisztrátori jogosultsága a film szerkesztéséhez.", "Hozzáférés megtagadva", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void DeleteMovieButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            if (app?.LoggedInUser?.isAdmin != true)
            {
                MessageBox.Show("Nincs adminisztrátori jogosultsága a film törléséhez.", "Hozzáférés megtagadva", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string? adminToken = app.AdminToken;

            if (string.IsNullOrEmpty(adminToken))
            {
                MessageBox.Show("Nincs bejelentkezve vagy érvényes token.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Biztosan törli a filmet?", "Megerősítés", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var (success, message) = await ApiService.DeleteMovie(_movieId, adminToken);
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
}