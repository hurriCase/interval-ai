using System;
using Newtonsoft.Json;

namespace Client.Scripts.DB.Entities.Base
{
    internal sealed class EntryData<TContent> where TContent : class
    {
        [JsonProperty("id")] public string Id { get; set; } = Guid.NewGuid().ToString();
        [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [JsonProperty("updatedAt")] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [JsonProperty("content")] public TContent Content { get; set; }
    }
}