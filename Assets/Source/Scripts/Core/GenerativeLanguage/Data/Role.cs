using System;
using System.Runtime.Serialization;

namespace Source.Scripts.Core.GenerativeLanguage.Data
{
    [Serializable]
    internal enum Role
    {
        [EnumMember(Value = "user")] User,
        [EnumMember(Value = "model")] Model
    }
}