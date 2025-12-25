using Eflatun.SceneReference;

namespace Source.Scripts.Core.References.Base
{
    internal interface ISceneReferences
    {
        SceneReference Splash { get; }
        SceneReference Main { get; }
        SceneReference Onboarding { get; }
    }
}