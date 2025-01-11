using System.Threading.Tasks;

namespace Client.Scripts.Core
{
    internal interface IUserDataController
    {
        void Init();
        void InitAsGuest();
        Task TransitionFromGuestToAuthenticated(string authenticatedUserId);
    }
}