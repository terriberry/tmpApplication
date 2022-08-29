using CoreApplicationFilterVal_10.Domain.Common.Filters;
using CoreApplicationFilterVal_10.Domain.Common.Filters.Attributes;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Entities.Filters.EntityFilter", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Models.Entities.Owners
{
    public class OwnerFilter : ListFilter<Owner>
    {
        [ContainsFilter]
        public string Name { get; set; }

        [LessThanFilter]
        public int? Age { get; set; }


    }
}