using System;
using System.Collections.Generic;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.Base.Validation;
using Client.Scripts.DB.Entities.Word;
using Newtonsoft.Json;

namespace Client.Scripts.DB.Entities.UserCategory
{
    [Serializable]
    internal sealed class UserCategoryEntryContent
    {
        [JsonProperty("title")] [Validation] public string Title { get; set; }

        [JsonProperty("description")]
        [Validation]
        public string Description { get; set; }

        [JsonProperty("words")] [Validation] public List<EntryData<WordEntryContent>> Words { get; set; } = new();
    }
}