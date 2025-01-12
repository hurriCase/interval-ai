using System;
using Client.Scripts.DB.Entities.Base.Validation;
using Newtonsoft.Json;

namespace Client.Scripts.DB.Entities.User
{
    [Serializable]
    internal sealed class UserEntryContent
    {
        [JsonProperty("userName")]
        [StringLength(1, 25)]
        [Required]
        public string UserName { get; set; }

        [JsonProperty("password")]
        [StringLength(6, 25)]
        [Required]
        public string Password { get; set; }

        [JsonProperty("email")]
        [StringLength(5, 100)]
        [Required]
        public string Email { get; set; }
    }
}