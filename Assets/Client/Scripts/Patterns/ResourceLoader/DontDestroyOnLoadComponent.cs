using UnityEngine;

namespace Client.Scripts.ResourceLoader
{
    internal sealed class DontDestroyOnLoadComponent : MonoBehaviour
    {
        internal bool IsInstantiated { get; set; }
    }
}