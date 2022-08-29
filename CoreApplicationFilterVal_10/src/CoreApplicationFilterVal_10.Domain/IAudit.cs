using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Entities.Audit.AuditInterface", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain
{
    public interface IAudit
    {
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}