using Client.Scripts.Database.Base;

namespace Client.Scripts.Database.Entities
{
    internal sealed class UserEntity : DBEntityBase<UserEntityData>
    {
        protected override string GetPath() => "users";
    }

    internal sealed class UserEntityData
    {
        internal string UserName { get; set; }
        internal string Password { get; set; }
        internal string Email { get; set; }
    }
}