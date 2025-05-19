namespace mozirendszer
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public bool isAdmin { get; set; } // Ez a mező kell a jogosultság ellenőrzéséhez
    }
}