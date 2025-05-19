using System.Windows;

namespace mozirendszer
{
    public partial class App : Application
    {
        public string? AdminToken { get; set; }
        public User? LoggedInUser { get; set; }
    }
}
