using Eflatun.SceneReference;
using Source.Scripts.Core.References.Base;
using UnityEngine;

namespace Source.Scripts.Core.References
{
    internal sealed class SceneReferences : ScriptableObject, ISceneReferences
    {
        [field: SerializeField] public SceneReference MainMenuScene { get; private set; }
        [field: SerializeField] public SceneReference Onboarding { get; private set; }
    }
}