using UnityEngine;

namespace Source.Scripts.Core.Others
{
    internal static class ComponentExtensions
    {
        public static bool IsInFrontOf<TSource, TTarget>(this TSource component, TTarget target)
            where TSource : Component
            where TTarget : Component
            => component.transform.GetSiblingIndex() > target.transform.GetSiblingIndex();
    }
}