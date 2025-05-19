using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mozirendszer
{
    public partial class MovieSearchWindow : Window
    {
        private ObservableCollection<Movie> SearchResults { get; set; } = new ObservableCollection<Movie>();

        public MovieSearchWindow()
        {
            InitializeComponent();
            SearchResultsDataGrid.ItemsSource = SearchResults;
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var movies = await ApiService.SearchMoviesByTitle(searchTerm);
                if (movies != null)
                {
                    SearchResults.Clear();
                    foreach (var movie in movies)
                    {
                        SearchResults.Add(movie);
                    }
                }
                else
                {
                    MessageBox.Show("Hiba a keresés során.");
                }
            }
            else
            {
                MessageBox.Show("Kérjük, adja meg a keresendő film címet.");
            }
        }

        private void ShowMovieDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchResultsDataGrid.SelectedItem is Movie selectedMovie)
            {
                var detailsWindow = new MovieDetailWindow(selectedMovie.Id);
                detailsWindow.Show();
            }
            else
            {
                MessageBox.Show("Kérjük, válasszon ki egy filmet a részletek megtekintéséhez.");
            }
        }
    }
}