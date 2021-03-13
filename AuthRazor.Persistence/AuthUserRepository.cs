using AuthRazor.Core;
using AuthRazor.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AuthRazor.Persistence
{
    public class AuthUserRepository : IAuthUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserAsync(AuthUser user) =>
            await _dbContext.AuthUsers.AddAsync(user);

        public async Task<AuthUser> FindByEmailAsync(string email)
            => await _dbContext.AuthUsers
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
