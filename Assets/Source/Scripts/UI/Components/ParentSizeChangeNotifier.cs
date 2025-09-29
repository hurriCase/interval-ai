using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Components
{
    internal sealed class ParentSizeChangeNotifier : MonoBehaviour
    {
        private void OnRectTransformDimensionsChange()
        {
            var parent = transform.parent;
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent as RectTransform);
        }
    }
}