using UnityEngine;

namespace Client.Scripts.Patterns.DontDestroyLoader
{
    /// <summary>
    /// Marker component to identify GameObjects that should persist between scenes.
    /// Add this component to any prefab that should be automatically instantiated
    /// and marked as DontDestroyOnLoad at runtime.
    /// </summary>
    internal sealed class DontDestroyOnLoadComponent : MonoBehaviour { }
}