using System;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreApplicationFilterVal_10.Domain.Contracts.Repositories;
using CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;
using CoreApplicationFilterVal_10.Persistence.Common;
using CoreApplicationFilterVal_10.Persistence.Persistence;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("TD.Repositories.EF.ReadWriteRepositories", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Persistence.Repositories;

public class OwnerReadWriteRepository : ReadWriteRepository<int, Owner>, IOwnerReadWriteRepository
{
    public OwnerReadWriteRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

}
