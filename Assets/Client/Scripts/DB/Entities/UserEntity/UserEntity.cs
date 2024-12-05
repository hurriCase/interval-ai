using Client.Scripts.DB.Entities.Base;

namespace Client.Scripts.DB.Entities.UserEntity
{
    [SingleInstanceEntry]
    internal sealed class UserEntity : EntityBase<UserEntryContent>
    {
        protected override string EntityPath => "user_entity";
    }
}