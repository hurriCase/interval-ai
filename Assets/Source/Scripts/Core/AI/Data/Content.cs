using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Source.Scripts.Core.AI.Data
{
    [Serializable]
    internal sealed class Content
    {
        [JsonProperty("parts")] public Part[] Parts { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("role")] public Role Role { get; private set; }

        internal Content(string text, Role role)
        {
            Parts = new[] { new Part(text) };
            Role = role;
        }

        [JsonConstructor]
        internal Content(Part[] parts, Role role)
        {
            Parts = parts;
            Role = role;
        }
    }
}