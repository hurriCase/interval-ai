using System;
using Client.Scripts.DB.Data;
using CustomUtils.Runtime.CustomTypes.Singletons;
using DependencyInjection.Runtime.InjectableMarkers;
using DependencyInjection.Runtime.InjectionBase;
using UnityEngine;

namespace Client.Scripts.Core.SignIn
{
    internal sealed class UnitySignInController : Singleton<UnitySignInController>, IInjectable,
        IAuthorizationController
    {
        [Inject] private readonly IUserDataController _userDataController;

        private string _mockUserId;

        public UnitySignInController()
        {
            InjectDependencies();
        }

        public void SignIn()
        {
            Debug.Log("[DevAuthController::SignIn] Development sign in activated");

            _mockUserId = "dev_" + Guid.NewGuid();

            HandleSuccessfulSignIn();
        }

        public void SignOut()
        {
            Debug.Log("[DevAuthController::SignOut] Development sign out");

            _mockUserId = null;

            _userDataController.InitAsGuest();
        }

        private async void HandleSuccessfulSignIn()
        {
            try
            {
                var wasGuest = UserData.Instance.LogInType == LogInType.Guest;

                if (wasGuest)
                    await _userDataController.TransitionFromGuestToAuthenticated(_mockUserId);
                else
                {
                    UserData.Instance.LogInType = LogInType.GoogleSignIn;
                    UserData.Instance.UserID = _mockUserId;
                }

                Debug.Log(
                    $"[DevAuthController::HandleSuccessfulSignIn] Successfully signed in with mock user: {_mockUserId}");
            }
            catch (Exception e)
            {
                Debug.LogError("[UnitySignInController::HandleSuccessfulSignIn] " +
                               $"Failed to signed in with mock user, with error: {e.Message}");
            }
        }

        public void InjectDependencies()
        {
            DependencyInjector.InjectDependencies(this);
        }
    }
}