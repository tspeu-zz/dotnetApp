using DatingApp.API.Models;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password); 
        
        Task<User> Login(string user, string password);

        Task<bool> UserExits(string password);

    }
}