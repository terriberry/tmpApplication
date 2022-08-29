using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Common.DateTime.DateTimeUtil", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Utils
{
    public static class DateTimeUtil
    {
        public static DateTime Now => DateTime.UtcNow;
    }
}