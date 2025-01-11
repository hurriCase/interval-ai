using System;
using System.Collections.Generic;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.Base.Validation;
using Client.Scripts.DB.Entities.WordEntity;
using Newtonsoft.Json;

namespace Client.Scripts.DB.Entities.CategoryEntity
{
    [Serializable]
    internal sealed class UserCategoryEntryContent
    {
        [JsonProperty("title")]
        [StringLength(3, 50)]
        [Required]
        public string Title { get; set; }

        [JsonProperty("description")]
        [StringLength(3, 250)]
        public string Description { get; set; }

        [JsonProperty("words")] public List<EntryData<WordEntryContent>> Words { get; set; } = new();
    }
}