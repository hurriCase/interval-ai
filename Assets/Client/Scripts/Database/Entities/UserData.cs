namespace Client.Scripts.Database.User
{
    internal sealed class UserEntityData
    {
        internal string UserName { get; set; }
        internal string Password { get; set; }
        internal string Email { get; set; }
    }

    internal sealed class UserEntity : DataBaseEntity<UserEntityData>, IInitializable
    {
        protected override string GetPath(string userId) => $"Users/{userId}";
    }
}