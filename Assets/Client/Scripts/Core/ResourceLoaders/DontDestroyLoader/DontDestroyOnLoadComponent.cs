using UnityEngine;

namespace Client.Scripts.Patterns.ResourceLoader
{
    internal sealed class DontDestroyOnLoadComponent : MonoBehaviour
    {
        internal bool IsInstantiated { get; set; }
    }
}