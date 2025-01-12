using System.Threading.Tasks;
using Client.Scripts.Core.MVC.Base;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.EntityController;
using Client.Scripts.DB.Entities.User;

namespace Client.Scripts.Core.MVC.User
{
    internal sealed class UserController : ControllerBase<UserEntity, UserEntryContent, UserModel>
    {
        public UserController(IEntityController entityController, IView<UserModel> view)
            : base(entityController, view) { }

        protected override UserModel CreateModel(EntryData<UserEntryContent> data)
            => new(data);

        public async Task<bool> CreateUser(string userName, string password, string email)
        {
            var content = new UserEntryContent
            {
                UserName = userName,
                Password = password,
                Email = email
            };

            return await CreateEntry(content);
        }
    }
}