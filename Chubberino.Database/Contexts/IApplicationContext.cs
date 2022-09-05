using Chubberino.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chubberino.Database.Contexts;

public interface IApplicationContext : IDisposable
{
    DbSet<Player> Players { get; set; }

    DbSet<StartupChannel> StartupChannels { get; set; }

    DbSet<UserCredentials> UserCredentials { get; set; }

    DbSet<ApplicationCredentials> ApplicationCredentials { get; set; }

    DbSet<Boss> Bosses { get; set; }

    DbSet<Emote> Emotes { get; set; }

    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

    Int32 SaveChanges();

    //
    // Summary:
    //     Saves all changes made in this context to the database.
    //     This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
    //     to discover any changes to entity instances before saving to the underlying database.
    //     This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
    //     Multiple active operations on the same context instance are not supported. Use
    //     'await' to ensure that any asynchronous operations have completed before calling
    //     another method on this context.
    //
    // Parameters:
    //   cancellationToken:
    //     A System.Threading.CancellationToken to observe while waiting for the task to
    //     complete.
    //
    // Returns:
    //     A task that represents the asynchronous save operation. The task result contains
    //     the number of state entries written to the database.
    //
    // Exceptions:
    //   T:Microsoft.EntityFrameworkCore.DbUpdateException:
    //     An error is encountered while saving to the database.
    //
    //   T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException:
    //     A concurrency violation is encountered while saving to the database. A concurrency
    //     violation occurs when an unexpected number of rows are affected during save.
    //     This is usually because the data in the database has been modified since it was
    //     loaded into memory.
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
