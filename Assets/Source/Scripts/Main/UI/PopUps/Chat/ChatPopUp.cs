using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Main.UI.PopUps.Chat.Behaviours;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.PopUps.Chat
{
    internal sealed class ChatPopUp : PopUpBase
    {
        [SerializeField] private MessageItem _messageItem;
        [SerializeField] private AspectRatioFitter _spacing;
        [SerializeField] private float _spacingRatio;

        [SerializeField] private RectTransform _chatContainer;

        [SerializeField] private TMP_InputField _messageInputField;
        [SerializeField] private ButtonComponent _sendMessageButton;

        internal override void Init()
        {
            _sendMessageButton.OnClickAsObservable().SubscribeAndRegister(this, static self => self.SendMessage());
        }

        private void SendMessage()
        {
            var typedText = _messageInputField.text;

            if (typedText.IsValid() is false)
                return;

            CreateMessage(typedText, MessageSourceType.User);
            CreateMessage("temp message", MessageSourceType.AI);
        }

        private void CreateMessage(string text, MessageSourceType sourceType)
        {
            var createdMessage = Instantiate(_messageItem, _chatContainer);
            createdMessage.Init(_chatContainer, text, sourceType);

            _spacing.CreateWidthSpacing(_spacingRatio, _chatContainer);
        }
    }
}