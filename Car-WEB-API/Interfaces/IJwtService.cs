using Car_WEB_API.Model;

namespace Car_WEB_API.Interfaces
{
    public interface IJwtService
    {
        string CreateJwt(User user);
        string RequestJwt(User user);
    }
}
