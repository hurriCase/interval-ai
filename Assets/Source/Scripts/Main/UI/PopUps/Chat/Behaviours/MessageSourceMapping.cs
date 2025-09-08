using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using Source.Scripts.Core.Others;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Chat.Behaviours
{
    [CreateAssetMenu(
        fileName = nameof(MessageSourceMapping),
        menuName = MenuPaths.MappingsPath + nameof(MessageSourceMapping)
    )]
    internal sealed class MessageSourceMapping : ThemeStateMappingGeneric<MessageSourceType> { }
}