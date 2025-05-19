using System.Windows;
using System.Collections.ObjectModel;

namespace mozirendszer
{
    public partial class MovieListWindow : Window
    {
        private ObservableCollection<Movie> Movies { get; set; } = new ObservableCollection<Movie>();
        public User user1;
        public string toeken1;

        public MovieListWindow(User user,string token)
        {
            InitializeComponent();
            MoviesDataGrid.ItemsSource = Movies;
            LoadMovies();
            user1 = user;
            toeken1 = token;

            var app = Application.Current as App;
            if (app?.LoggedInUser?.isAdmin == true)
            {
                CreateMovieButton.Visibility = Visibility.Visible;
            }
            else
            {
                CreateMovieButton.Visibility = Visibility.Collapsed;
            }
        }
        private void ShowMovieProfilButton_Click(object sender, RoutedEventArgs e)
        {
           
                var detailsWindow = new UserProfileWindow(user1, toeken1);
                detailsWindow.Show();
 
        }
        public async void LoadMovies()
        {
            var movies = await ApiService.GetAllMovies();
            if (movies != null)
            {
                Movies.Clear();
                foreach (var movie in movies)
                {
                    Movies.Add(movie);
                }
            }
            else
            {
                MessageBox.Show("Hiba a filmek betöltésekor.");
            }
        }

        private void ShowMovieDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesDataGrid.SelectedItem is Movie selectedMovie)
            {
                var detailsWindow = new MovieDetailWindow(selectedMovie.Id);
                detailsWindow.Owner = this; 
                detailsWindow.Show();
            }
            else
            {
                MessageBox.Show("Kérjük, válasszon ki egy filmet a részletek megtekintéséhez.");
            }
        }

        private void CreateMovieButton_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new MovieCreateWindow();
            createWindow.Owner = this; 
            createWindow.ShowDialog(); 
            LoadMovies(); 
        }

        private void SearchMovieButton_Click(object sender, RoutedEventArgs e)
        {
            var searchWindow = new MovieSearchWindow();
            searchWindow.Show();
        }
    }
}