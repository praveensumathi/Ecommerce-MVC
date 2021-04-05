
namespace EcommerceMVC.Auth
{
    public interface IJwtAuthManager
    {
        public string Authenticate(string name, string password);
        
    }
}
