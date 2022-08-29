using System.Threading;
using System.Threading.Tasks;
using CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.DbContextInterface", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Owner> Owners { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}