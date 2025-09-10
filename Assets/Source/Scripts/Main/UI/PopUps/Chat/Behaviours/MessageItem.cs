using CustomUtils.Runtime.CustomBehaviours;
using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Chat.Behaviours
{
    internal sealed class MessageItem : RectTransformBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private ButtonComponent _pinButton;
        [SerializeField] private ButtonComponent _translateButton;

        internal void Init(string message)
        {
            _messageText.text = message;
        }
    }
}