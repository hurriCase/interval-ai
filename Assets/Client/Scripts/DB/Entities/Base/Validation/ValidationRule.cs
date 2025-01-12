using System.Collections.Generic;
using Client.Scripts.Patterns.Extensions;
using Newtonsoft.Json;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    internal sealed class ValidationRule
    {
        [JsonProperty("propertyName")] internal string PropertyName { get; set; }

        [JsonProperty("validationType")] internal string ValidationTypeString => ValidationType.GetJsonPropertyName();

        [JsonIgnore] internal ValidationType ValidationType { get; set; }

        [JsonProperty("parameters")] internal Dictionary<string, object> Parameters { get; set; } = new();
    }
}