using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.GenerativeLanguage;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.UI.PopUps.Chat
{
    internal sealed class ChatPopUp : PopUpBase
    {
        [SerializeField] private MessageItem _aiMessageItem;
        [SerializeField] private MessageItem _userMessageItem;

        [SerializeField] private RectTransform _contentContainer;

        [SerializeField] private TMP_InputField _messageInputField;
        [SerializeField] private ButtonComponent _sendMessageButton;

        private IGenerativeLanguage _generativeLanguage;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(IGenerativeLanguage generativeLanguage, IObjectResolver objectResolver)
        {
            _generativeLanguage = generativeLanguage;
            _objectResolver = objectResolver;
        }

        internal override void Init()
        {
            _sendMessageButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self => self.SendMessage());
        }

        private void SendMessage()
        {
            var typedText = _messageInputField.text;

            if (string.IsNullOrEmpty(typedText))
                return;

            _messageInputField.text = string.Empty;

            var createdMessage = _objectResolver.Instantiate(_userMessageItem, _contentContainer);
            createdMessage.Init(typedText);

            HandleUserMessage(typedText).Forget();
        }

        private async UniTask HandleUserMessage(string text)
        {
            var response =
                await _generativeLanguage.SendPromptWithChatHistoryAsync(text, destroyCancellationToken);

            var createdMessage = _objectResolver.Instantiate(_aiMessageItem, _contentContainer);
            createdMessage.Init(response);
        }
    }
}