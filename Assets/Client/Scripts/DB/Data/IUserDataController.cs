using System.Threading.Tasks;

namespace Client.Scripts.DB.Data
{
    internal interface IUserDataController
    {
        void Init();
        void InitAsGuest();
        Task TransitionFromGuestToAuthenticated(string authenticatedUserId);
    }
}