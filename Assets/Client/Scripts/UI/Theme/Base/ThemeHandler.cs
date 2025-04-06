using AssetLoader.Runtime;
using CustomClasses.Runtime.Singletons;
using UnityEngine;

namespace Client.Scripts.UI.Theme.Base
{
    [CreateAssetMenu(fileName = "ThemeHandler", menuName = "UI/ThemeHandler")]
    [Resource("Assets/Client/Scriptables/Resources/UI/Theme", nameof(ThemeHandler), "UI/Theme")]
    internal sealed class ThemeHandler : SingletonScriptableObject<ThemeHandler>
    {
        [field: SerializeField] internal ThemeType CurrentThemeType { get; set; }
    }
}