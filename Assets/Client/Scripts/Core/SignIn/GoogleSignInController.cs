using System.Threading.Tasks;
using Client.Scripts.DB.Data;
using Client.Scripts.Patterns.DI.Base;
using Google;
using UnityEngine;

namespace Client.Scripts.Core.SignIn
{
    internal sealed class GoogleSignInController : Injectable, IAuthorizationController
    {
        [Inject] private readonly IUserDataController _userDataController;

        public void SignIn()
        {
            var configuration = new GoogleSignInConfiguration
            {
                WebClientId = AppConfig.Instance.WebClientId,
                RequestIdToken = true,
                UseGameSignIn = false,
                RequestEmail = true
            };

            GoogleSignIn.Configuration = configuration;

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.Default);
        }

        public void SignOut()
        {
            GoogleSignIn.DefaultInstance.SignOut();

            _userDataController.InitAsGuest();
        }

        private async void OnAuthenticationFinished(Task<GoogleSignInUser> task)
        {
            if (task.IsFaulted)
            {
                HandleSignInError(task);
                return;
            }

            if (task.IsCompleted && task.Result != null)
                await HandleSuccessfulSignIn(task.Result);
        }

        private async Task HandleSuccessfulSignIn(GoogleSignInUser user)
        {
            var wasGuest = UserData.Instance.LogInType == LogInType.Guest;

            if (wasGuest)
                await _userDataController.TransitionFromGuestToAuthenticated(user.UserId);
            else
            {
                UserData.Instance.LogInType = LogInType.GoogleSignIn;
                UserData.Instance.UserID = user.UserId;
            }

            Debug.Log($"[GoogleSignInController::HandleSuccessfulSignIn] Successfully signed in user: {user.UserId}");
        }

        private void HandleSignInError(Task<GoogleSignInUser> task)
        {
            if (task.Exception == null) return;

            using var enumerator = task.Exception.InnerExceptions.GetEnumerator();
            if (enumerator.MoveNext())
            {
                if (enumerator.Current is GoogleSignIn.SignInException error)
                    Debug.LogError(
                        $"[GoogleSignInController::HandleSignInError] Sign-in error: {error.Status} - {error.Message}");
            }
            else
                Debug.LogError(
                    $"[GoogleSignInController::HandleSignInError] Unexpected sign-in exception: {task.Exception}");

            if (string.IsNullOrEmpty(UserData.Instance.UserID))
                _userDataController.InitAsGuest();
        }
    }
}