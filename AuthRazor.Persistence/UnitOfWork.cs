using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AuthRazor.Core;
using AuthRazor.Core.Contracts;
//using Devices.Core.Contracts.Authentication;
//using IdentityTutorial.Web.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthRazor.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private bool _disposed;

        public UnitOfWork()
        {
            _dbContext = new ApplicationDbContext();
            AuthUsers = new AuthUserRepository(_dbContext);
        }

        public IAuthUserRepository AuthUsers { get; }

        public async Task DeleteDatabaseAsync()
        {
            await _dbContext.Database.EnsureDeletedAsync();
        }

        public async Task MigrateDatabaseAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            var entities = _dbContext.ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added
                                 || entity.State == EntityState.Modified)
                .Select(e => e.Entity)
                .ToArray();  // Geänderte Entities ermitteln
            foreach (var entity in entities)
            {
                Validator.ValidateObject(entity, new ValidationContext(entity), true);
                ValidationResult result;
                if (entity is AuthUser)
                {     // UnitOfWork injizieren, wenn Interface implementiert ist
                    var validationContext = new ValidationContext(entity, null, null);
                    validationContext.InitializeServiceProvider(serviceType => this);

                    var validationResults = new List<ValidationResult>();
                    var isValid = Validator.TryValidateObject(entity, validationContext, validationResults,
                        validateAllProperties: true);
                    if (!isValid)
                    {
                        var memberNames = new List<string>();
                        List<ValidationException> validationExceptions = new List<ValidationException>();
                        foreach (ValidationResult validationResult in validationResults)
                        {
                            validationExceptions.Add(new ValidationException(validationResult, null, null));
                            memberNames.AddRange(validationResult.MemberNames);
                        }

                        if (validationExceptions.Count == 1)  // eine Validationexception werfen
                        {
                            throw validationExceptions.Single();
                        }
                        else  // AggregateException mit allen ValidationExceptions als InnerExceptions werfen
                        {
                            throw new ValidationException($"Entity validation failed for {string.Join(", ", memberNames)}",
                                new AggregateException(validationExceptions));
                        }
                    }
                }
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    await _dbContext.DisposeAsync();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}

