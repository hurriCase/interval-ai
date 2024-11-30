using Client.Scripts.Steps;
using UnityEngine;

namespace Client.Scripts
{
    internal sealed class StartUpController : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void OnBeforeSceneLoadRuntimeMethod()
        {
            var diStep = new DIStep();
            diStep.OnCompleted += ShowStep;
            await diStep.Execute(0);

            var steps = new IStep[]
            {
                new DBStep(),
                new SceneContextStep(),
                new FireBaseStep()
            };

            for (var i = 0; i < steps.Length; i++)
            {
                steps[i].OnCompleted += ShowStep;
                await steps[i].Execute(i + 1);
            }

            await SceneLoader.LoadLoginScene();
        }

        //TODO:<dmitriy.sukharev> temporary solution
        private static void ShowStep(int step, string stepName)
        {
            Debug.Log($"[StartUpController::ShowStep] {step} step is initialized at {stepName}");
        }
    }
}