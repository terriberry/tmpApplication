using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreApplicationFilterVal_10.Application.Common.Interfaces;
using CoreApplicationFilterVal_10.Domain;
using CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;
using CoreApplicationFilterVal_10.Domain.Utils;
using CoreApplicationFilterVal_10.Persistence.Persistence.Configurations;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.DbContext", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Persistence.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Owner> Owners { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            var updatedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Select(e => e.Entity).OfType<IAudit>();
            foreach (var entity in updatedEntities)
            {
                entity.LastModifiedAt = DateTimeUtil.Now;
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureModel(modelBuilder);

            modelBuilder.ApplyConfiguration(new OwnerConfiguration());
        }

        [IntentManaged(Mode.Ignore)]
        private void ConfigureModel(ModelBuilder modelBuilder)
        {
            // Customize Default Schema
            // modelBuilder.HasDefaultSchema("CoreApplicationFilterVal_10");

            // Seed data
            // https://rehansaeed.com/migrating-to-entity-framework-core-seed-data/
            /* Eg.

            modelBuilder.Entity<Car>().HasData(
                new Car() { CarId = 1, Make = "Ferrari", Model = "F40" },
                new Car() { CarId = 2, Make = "Ferrari", Model = "F50" },
                new Car() { CarId = 3, Make = "Labourghini", Model = "Countach" });
            */
        }
    }
}