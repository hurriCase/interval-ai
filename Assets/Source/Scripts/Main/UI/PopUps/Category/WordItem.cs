using R3;
using R3.Triggers;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Category
{
    internal sealed class WordItem : MonoBehaviour
    {
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;
        [SerializeField] private TextMeshProUGUI _learningWordText;
        [SerializeField] private TextMeshProUGUI _translationsText;
        [SerializeField] private ButtonComponent _wordInfoButton;

        [Inject] private IWindowsController _windowsController;

        private WordEntry _currentWordEntry;

        internal void Init(WordEntry wordEntry)
        {
            _currentWordEntry = wordEntry;

            _learningWordText.text = wordEntry.LearningWord;
            _translationsText.text = wordEntry.NativeWord;

            _wordProgressBehaviour.Init();
            _wordProgressBehaviour.UpdateProgress(wordEntry);

            _wordInfoButton.OnCancelAsObservable()
                .Subscribe(this, static (_, self) => self.OpenWordInfo())
                .RegisterTo(destroyCancellationToken);
        }

        private void OpenWordInfo()
            => _windowsController.OpenPopUpByType(PopUpType.WordInfo, _currentWordEntry);
    }
}