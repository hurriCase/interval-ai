using System;
using CustomUtils.Runtime.Extensions;
using Newtonsoft.Json;

namespace Source.Scripts.Core.AI.Data
{
    [Serializable]
    internal sealed class Content
    {
        [JsonProperty("parts")] public Part[] Parts { get; private set; }
        [JsonProperty("role")] public string Role { get; private set; }

        internal Content(string text, Role role)
        {
            Parts = new[] { new Part(text) };
            Role = role.GetJsonPropertyName();
        }

        [JsonConstructor]
        internal Content(Part[] parts, string role)
        {
            Parts = parts;
            Role = role;
        }
    }
}