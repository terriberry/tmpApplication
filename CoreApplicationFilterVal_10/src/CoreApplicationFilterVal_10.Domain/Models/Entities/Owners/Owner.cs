using System;
using CoreApplicationFilterVal_10.Domain.Common.Contracts;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Entities.Entity", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;

public class Owner : IIdEntity<int>
{

    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

}
