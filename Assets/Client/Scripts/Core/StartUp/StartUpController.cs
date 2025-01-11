using Client.Scripts.Core.StartUp.Steps;
using UnityEngine;

namespace Client.Scripts.Core.StartUp
{
    internal sealed class StartUpController : MonoBehaviour
    {
        internal static bool IsInited { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void OnBeforeSceneLoadRuntimeMethod()
        {
            if (IsInited)
                return;

            var diStep = new DIStep();
            diStep.OnCompleted += ShowStep;
            await diStep.Execute(0);

            var steps = new IStep[]
            {
                new DataStep(),
                new AIStep(),
                new SceneContextStep(),
                new FireBaseStep()
            };

            for (var i = 0; i < steps.Length; i++)
            {
                steps[i].OnCompleted += ShowStep;
                await steps[i].Execute(i + 1);
            }

            IsInited = true;

            //TODO:<dmitriy.sukharev> think about how to better load scene
            //TODO:to prevent scene load from not startup scene
            //await SceneLoader.LoadLoginScene();
        }

        //TODO:<dmitriy.sukharev> temporary solution
        private static void ShowStep(int step, string stepName)
        {
            Debug.Log($"[StartUpController::ShowStep] {step} step is initialized at {stepName}");
        }
    }
}