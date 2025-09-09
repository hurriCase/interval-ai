using Newtonsoft.Json;

namespace Source.Scripts.Core.AI.Data
{
    internal enum Role
    {
        [JsonProperty("user")] User,
        [JsonProperty("model")] Model
    }
}