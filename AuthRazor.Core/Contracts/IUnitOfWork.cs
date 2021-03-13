using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthRazor.Core.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthUserRepository AuthUsers { get; }
        Task<int> SaveChangesAsync();
        Task DeleteDatabaseAsync();
        Task MigrateDatabaseAsync();
    }
}
