using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Common.NotFoundException", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Exceptions;

[Serializable]
public class NotFoundException : Exception
{
    public string Entity { get; }
    public string Identifier { get; }

    public NotFoundException(string entity, string identifier) : base($"Could not find [{entity}] with id [{identifier}]")
    {
        Entity = entity;
        Identifier = identifier;
    }
}