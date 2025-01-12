using System;
using Client.Scripts.DB.Entities.Base.Validation;
using Newtonsoft.Json;

namespace Client.Scripts.DB.Entities.User
{
    [Serializable]
    internal sealed class UserEntryContent
    {
        [JsonProperty("userName")]
        [Validation]
        public string UserName { get; set; }

        [JsonProperty("password")]
        [Validation]
        public string Password { get; set; }

        [JsonProperty("email")] [Validation] public string Email { get; set; }
    }
}