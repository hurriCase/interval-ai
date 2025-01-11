using Client.Scripts.Core.SignIn;
using Client.Scripts.Patterns.DI.Base;

namespace Client.Scripts.UI
{
    internal sealed class SignInWindow : InjectableBehaviour
    {
        [Inject] private IAuthorizationController _authorizationController;

        public void SignIn() => _authorizationController.SignIn();
        public void SignOut() => _authorizationController.SignOut();
    }
}