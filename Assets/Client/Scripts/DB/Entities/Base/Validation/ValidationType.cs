using Unity.Plastic.Newtonsoft.Json;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    internal enum ValidationType
    {
        [JsonProperty("required")] Required,
        [JsonProperty("stringLength")] StringLength,
        [JsonProperty("numericRange")] NumericRange
    }
}