using System.Threading.Tasks;

namespace AuthRazor.Core.Contracts
{
    public interface IAuthUserRepository
    {
        Task<AuthUser> FindByEmailAsync(string email);
        Task AddUserAsync(AuthUser user);
    }
}
