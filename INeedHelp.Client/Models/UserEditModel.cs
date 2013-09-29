namespace INeedHelp.Client.Models
{
    public class UserEditModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OldPasswordHash { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string NewPasswordHash { get; set; }
    }
}