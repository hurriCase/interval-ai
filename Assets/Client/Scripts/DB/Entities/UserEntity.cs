using System;
using Client.Scripts.DB.Base;
using UnityEngine;

namespace Client.Scripts.DB.Entities
{
    internal sealed class UserEntity : DBEntityBase<UserEntityData>
    {
        protected override string GetPath() => "users";
    }

    [Serializable]
    internal struct UserEntityData
    {
        [field: SerializeField] public string UserName { get; set; }
        [field: SerializeField] public string Password { get; set; }
        [field: SerializeField] public string Email { get; set; }
    }
}