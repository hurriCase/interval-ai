using System.Collections.Generic;
using System.Linq;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    internal sealed class ValidationResult
    {
        internal bool IsValid => !Errors.Any();
        internal List<string> Errors { get; } = new();
    }
}