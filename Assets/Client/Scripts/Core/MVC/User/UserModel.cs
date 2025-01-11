using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.UserEntity;
using Client.Scripts.MVC.Base;

namespace Client.Scripts.Core.MVC.User
{
    internal class UserModel : ModelBase<UserEntryContent>
    {
        internal string UserName => Data.Content.UserName;
        internal string Password => Data.Content.Password;
        internal string Email => Data.Content.Email;

        internal UserModel(EntryData<UserEntryContent> data) : base(data) { }
    }
}