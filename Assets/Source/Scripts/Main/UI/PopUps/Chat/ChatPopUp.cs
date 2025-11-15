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
        [SerializeField] private MessageItem _aiMessageItem;
        [SerializeField] private MessageItem _userMessageItem;

        [SerializeField] private RectTransform _contentContainer;

        [SerializeField] private TMP_InputField _messageInputField;
        [SerializeField] private ThemeButton _sendMessageButton;
        [SerializeField] private ThemeButton _audioRecordButton;

        private IGenerativeLanguage _generativeLanguage;
        private ISpeechRecognizer _speechRecognizer;
        private IObjectResolver _objectResolver;
        private ITranslator _translator;

        [Inject]
        internal void Inject(
            IGenerativeLanguage generativeLanguage,
            ISpeechRecognizer speechRecognizer,
            IObjectResolver objectResolver,
            ITranslator translator)
        {
            _generativeLanguage = generativeLanguage;
            _speechRecognizer = speechRecognizer;
            _objectResolver = objectResolver;
            _translator = translator;
        }

        internal override void Init()
        {
            _sendMessageButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.SendChatMessage(self._messageInputField.text));

            _speechRecognizer.OnRecognizedTextReceived
                .SubscribeUntilDestroy(this, static (text, self) => self.SendChatMessage(text));

            _audioRecordButton.OnClickAsObservable()
                .Scan(false, static (isListening, _) => isListening is false)
                .SubscribeUntilDestroy(this, static (isListening, self) => self.ToggleRecord(isListening));
        }

        internal override UniTask ShowAsync()
        {
            _translator.UpdateAvailable(destroyCancellationToken);

            return base.ShowAsync();
        }

        private void SendChatMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            _messageInputField.text = string.Empty;

            var createdMessage = _objectResolver.Instantiate(_userMessageItem, _contentContainer);
            createdMessage.Init(message);

            HandleUserMessage(message).Forget();
        }

        private async UniTask HandleUserMessage(string text)
        {
            var response =
                await _generativeLanguage.SendPromptWithChatHistoryAsync(text, destroyCancellationToken);

            var createdMessage = _objectResolver.Instantiate(_aiMessageItem, _contentContainer);
            createdMessage.Init(response);
        }

        private void ToggleRecord(bool isListening)
        {
            if (isListening)
            {
                _speechRecognizer.TryStartListening();
                return;
            }

            _speechRecognizer.StopListening();
        }
    }
}