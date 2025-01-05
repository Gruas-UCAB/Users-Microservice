using UsersMicroservice.src.user.domain;

namespace UsersMicroservice.src.auth.application.commands.login.types
{
    public class LoginUserResponse(User user, string accessToken, int expiresIn)
    {
        public User User = user;
        public string AccessToken = accessToken;
        public int ExpiresIn = expiresIn;
    }
}
