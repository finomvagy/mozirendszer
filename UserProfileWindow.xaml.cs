using System.Windows;

namespace mozirendszer
{
    public partial class UserProfileWindow : Window
    {
        public UserProfileWindow(User user, string token)
        {
            InitializeComponent();
            UsernameLabel.Content = user.Username;
            EmailLabel.Content = user.EmailAddress;
        }
    }
}
