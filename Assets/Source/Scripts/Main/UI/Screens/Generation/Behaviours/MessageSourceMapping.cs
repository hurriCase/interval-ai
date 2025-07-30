using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.Screens.Generation.Behaviours
{
    [CreateAssetMenu(fileName = nameof(MessageSourceMapping), menuName = nameof(MessageSourceMapping))]
    internal sealed class MessageSourceMapping : ThemeStateMappingGeneric<MessageSourceType> { }
}