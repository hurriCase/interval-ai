using System;
using Client.Scripts.DB.Base;

namespace Client.Scripts.DB.Entities
{
    internal sealed class UserEntity : DBEntityBase<UserEntityData>
    {
        protected override string GetPath() => "users";
    }

    [Serializable]
    internal sealed class UserEntityData
    {
        internal string UserName { get; set; }
        internal string Password { get; set; }
        internal string Email { get; set; }
    }
}