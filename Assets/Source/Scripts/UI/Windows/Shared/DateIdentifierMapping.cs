using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Shared
{
    [CreateAssetMenu(fileName = nameof(DateIdentifierMapping), menuName = nameof(DateIdentifierMapping))]
    internal sealed class DateIdentifierMapping : ThemeStateMappingGeneric<DateIdentifierColorType> { }
}