using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Audio.AudioRecord;
using Source.Scripts.Core.GenerativeLanguage;
using Source.Scripts.Core.Localization.Translator;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.UI.PopUps.Chat
{
    internal sealed class ChatPopUp : PopUpBase
    {
        [SerializeField] private ChatInputBehaviour _chatInputBehaviour;

        [SerializeField] private ThemeButton _sendMessageButton;
        [SerializeField] private MessageItem _aiMessageItem;
        [SerializeField] private MessageItem _userMessageItem;

        [SerializeField] private RectTransform _contentContainer;

        [SerializeField] private TMP_InputField _messageInputField;

        private IGenerativeLanguageService _generativeLanguageService;
        private ISpeechRecognizer _speechRecognizer;
        private IObjectResolver _objectResolver;
        private ITranslator _translator;

        [Inject]
        internal void Inject(
            IGenerativeLanguageService generativeLanguageService,
            ISpeechRecognizer speechRecognizer,
            IObjectResolver objectResolver,
            ITranslator translator)
        {
            _generativeLanguageService = generativeLanguageService;
            _speechRecognizer = speechRecognizer;
            _objectResolver = objectResolver;
            _translator = translator;
        }

        internal override void Init()
        {
            _chatInputBehaviour.Init();

            _sendMessageButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.SendChatMessage(self._messageInputField.text));

            _speechRecognizer.OnTextReceived
                .SubscribeUntilDestroy(this, static (text, self) => self.SendChatMessage(text));
        }

        internal override UniTask ShowAsync()
        {
            _translator.UpdateAvailable(destroyCancellationToken);
            _speechRecognizer.Init();

            return base.ShowAsync();
        }

        internal override UniTask HideAsync()
        {
            _speechRecognizer.Dispose();

            return base.HideAsync();
        }

        private void SendChatMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            _messageInputField.text = string.Empty;

            var createdMessage = _objectResolver.Instantiate(_userMessageItem, _contentContainer);
            createdMessage.Init(message);

            // HandleUserMessage(message).Forget();
        }

        private async UniTask HandleUserMessage(string text)
        {
            var response =
                await _generativeLanguageService.SendPromptWithChatHistoryAsync(text, destroyCancellationToken);

            var createdMessage = _objectResolver.Instantiate(_aiMessageItem, _contentContainer);
            createdMessage.Init(response);
        }
    }
}