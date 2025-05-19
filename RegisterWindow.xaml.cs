using System.Windows;

namespace mozirendszer
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var reg = new MainWindow();
            reg.Show();
            this.Close();
            var (success, message) = await ApiService.Register(UsernameBox.Text, EmailBox.Text, PasswordBox.Password);
            MessageBox.Show(message);
        }
    }
}
