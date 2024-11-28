using UnityEngine;

namespace Client.Scripts
{
    internal static class PrefabLoader
    {
        internal static GameObject LoadDontDestroyOnLoad() => Resources.Load<GameObject>("P_DontDestroyOnLoad");
    }
}