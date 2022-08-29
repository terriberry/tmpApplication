using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Entities.IdEntityInterface", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Contracts;

public interface IIdEntity<TK> : IEntity
    where TK : struct
{
    public TK Id { get; set; }
}
