using AssetLoader.Runtime;
using CustomClasses.Runtime.Singletons;
using UnityEngine;

namespace Client.Scripts.UI
{
    [Resource("Assets/Client/Scriptables/Resources/References", nameof(ShaderReferences), "References")]
    [CreateAssetMenu(fileName = "ShaderReferences", menuName = "ShaderReferences", order = 0)]
    internal sealed class ShaderReferences : SingletonScriptableObject<ShaderReferences>
    {
        [field: SerializeField] internal Shader GradientShader { get; set; }
    }
}