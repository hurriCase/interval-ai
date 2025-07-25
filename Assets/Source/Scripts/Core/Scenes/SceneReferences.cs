using Eflatun.SceneReference;
using UnityEngine;

namespace Source.Scripts.Core.Scenes
{
    internal sealed class SceneReferences : ScriptableObject, ISceneReferences
    {
        [field: SerializeField] public SceneReference MainMenuScene { get; private set; }
        [field: SerializeField] public SceneReference Onboarding { get; private set; }
    }
}