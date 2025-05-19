using System.Windows;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using System.Collections.ObjectModel;

namespace mozirendszer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var (success, message, user, token) = await ApiService.Login(EmailBox.Text, PasswordBox.Password);
            if (success)
            {
                // Tárold el a tokent és a felhasználót az App osztályban
                (Application.Current as App).AdminToken = token;
                (Application.Current as App).LoggedInUser = user;

                var movieListWindow = new MovieListWindow(user, token); // Most a MovieListWindow nyílik meg
                movieListWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Hiba: " + message);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var reg = new RegisterWindow();
            reg.Show();
            this.Close();
        }
    }
}
