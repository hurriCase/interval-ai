using UnityEngine;

namespace Client.Scripts.Patterns
{
    internal static class PrefabLoader
    {
        internal static GameObject LoadDontDestroyOnLoad() => Resources.Load<GameObject>("P_DontDestroyOnLoad");
    }
}