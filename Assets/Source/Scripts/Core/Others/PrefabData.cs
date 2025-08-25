using UnityEngine.UI;

namespace Source.Scripts.Core.Others
{
    internal readonly struct PrefabData<TPrefab>
    {
        internal TPrefab Prefab { get; }
        internal AspectRatioFitter Spacing { get; }

        internal PrefabData(TPrefab prefab, AspectRatioFitter spacing)
        {
            Prefab = prefab;
            Spacing = spacing;
        }
    }
}