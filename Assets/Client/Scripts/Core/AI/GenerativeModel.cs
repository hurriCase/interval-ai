using Client.Scripts.Core.AiController;
using Newtonsoft.Json;

namespace Client.Scripts.Core.AI
{
    internal sealed class GenerativeModel
    {
        [JsonProperty("apiUrl")]
        internal string EndpointFormat { get; set; }
            = "https://generativelanguage.googleapis.com/v1beta/models/{0}:generateContent?key={1}";

        [JsonProperty("modelName")] internal string ModelName { get; set; } = "gemini-1.5-flash-latest";

        [JsonProperty("apiKey")]
        internal string ApiKey { get; set; } = "AIzaSyCm0tkXkG-rsuENh6HgVdGDeOp8tIZACS4";

        [JsonProperty("generationConfig")]
        internal GenerationConfig GenerationConfig { get; set; } = new GenerationConfig();
    }
}