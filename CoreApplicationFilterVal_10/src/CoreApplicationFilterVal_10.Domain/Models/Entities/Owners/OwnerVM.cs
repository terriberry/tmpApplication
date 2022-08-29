using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoreApplicationFilterVal_10.Domain.Common.Mapping;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Services.Dto", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;

public class OwnerVM : IMapFrom<Owner>
{

    public int Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }
}
