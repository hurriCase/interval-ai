using UnityEngine;
using System.Threading.Tasks;
using Google;

internal sealed class AuthorizationController : MonoBehaviour
{
    private const string WebClientId = "488687700276-djos9qkmcv8glof35178j3fv8p4vritr.apps.googleusercontent.com";

    public void OnSignIn()
    {
        var configuration = new GoogleSignInConfiguration
        {
            WebClientId = WebClientId,
            RequestIdToken = true,
            UseGameSignIn = false,
            RequestEmail = true
        };

        GoogleSignIn.Configuration = configuration;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.Default);
    }

    public void OnSignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
    }

    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
            if (task.Exception != null)
            {
                using var enumerator = task.Exception.InnerExceptions.GetEnumerator();

                var error = (GoogleSignIn.SignInException)enumerator.Current;

                if (enumerator.MoveNext())
                {
                    if (error != null) 
                        Debug.LogError("Got Error: " + error.Status + " " + error.Message);
                }
                else
                    Debug.LogError("Got unexpected exception?!?" + task.Exception);
            }
    }
}