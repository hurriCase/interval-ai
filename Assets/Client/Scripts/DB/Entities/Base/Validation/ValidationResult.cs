using System.Collections.Generic;
using System.Linq;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    internal sealed class ValidationResult
    {
        internal bool IsValid => Errors.Any() is false;
        internal List<string> Errors { get; } = new();
    }
}