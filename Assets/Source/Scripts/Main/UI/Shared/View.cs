using UnityEngine;

namespace Source.Scripts.Main.UI.Shared
{
    internal abstract class View<TEntry> : MonoBehaviour
    {
        internal abstract void Init(TEntry entry);
        internal abstract void UpdateView(TEntry entry);
    }
}