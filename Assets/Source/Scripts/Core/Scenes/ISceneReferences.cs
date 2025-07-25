using Eflatun.SceneReference;

namespace Source.Scripts.Core.Scenes
{
    internal interface ISceneReferences
    {
        SceneReference MainMenuScene { get; }
        SceneReference Onboarding { get; }
    }
}