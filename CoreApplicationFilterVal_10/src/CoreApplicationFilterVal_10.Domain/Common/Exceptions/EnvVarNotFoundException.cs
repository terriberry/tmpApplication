using System;
using System.Runtime.Serialization;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Common.EnvVarNotFoundException", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Exceptions
{
    [Serializable]
    public class EnvVarNotFoundException : Exception
    {
        public EnvVarNotFoundException(string varName) : base($"Could not find environment variable {varName}") { }

        protected EnvVarNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}