namespace UsersMicroservice.src.auth.application.models
{
    public class Credentials(string id, string userId, string email, string password)
    {
        public string Id = id;
        public string UserId = userId;
        public string Email = email;
        public string Password = password;
    }
}
