using Microsoft.EntityFrameworkCore;
using Modulio.Application.Abstractions.Services;
using Modulio.Domain.Base;

namespace Modulio.Persistence.Context
{
    public class ModulioDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public ModulioDbContext(DbContextOptions<ModulioDbContext> options,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId ?? 0;
                    entry.Entity.CreatedAt = _dateTimeService.Now;
                    break;
                    case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModifiedAt = _dateTimeService.Now;
                    break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModulioDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
