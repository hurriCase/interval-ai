using CustomUtils.Runtime.Extensions;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.WordInfo;
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

            _learningWordText.text = wordEntry.Word.Learning;
            _translationsText.text = ZString.Join(", ", wordEntry.Word.Natives);

            _wordProgressBehaviour.Init();
            _wordProgressBehaviour.UpdateProgress(wordEntry);

            _wordInfoButton.OnClickAsObservable()
                .SubscribeAndRegister(this, static self => self.OpenWordInfo());
        }

        private void OpenWordInfo()
        {
            var wordInfoPopUp = _windowsController.OpenPopUp<WordInfoPopUp>();
            wordInfoPopUp.SetParameters(_currentWordEntry);
        }
    }
}