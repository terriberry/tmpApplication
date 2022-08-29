using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Common.ValidationOperationType", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Validation;

public enum ValidationOperationType
{
    Create,
    Update,
    Patch
}
