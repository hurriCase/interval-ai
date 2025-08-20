using R3;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.PopUp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.Screens.Generation.Behaviours
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
            _sendMessageButton.OnClickAsObservable()
                .Subscribe(this, static (_, behaviour) => behaviour.SendMessage())
                .RegisterTo(destroyCancellationToken);
        }

        private void SendMessage()
        {
            var typedText = _messageInputField.text;

            if (string.IsNullOrEmpty(typedText))
                return;

            CreateMessage(typedText, MessageSourceType.User);
            CreateMessage("temp message", MessageSourceType.AI);
        }

        private void CreateMessage(string text, MessageSourceType sourceType)
        {
            var createdMessage = Instantiate(_messageItem, _chatContainer);
            createdMessage.Init(_chatContainer, text, sourceType);

            var createdSpacing = Instantiate(_spacing, _chatContainer);
            createdSpacing.aspectRatio = _spacingRatio;
        }
    }
}