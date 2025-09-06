using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Shared;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    internal class PracticeModule : MonoBehaviour
    {
        [SerializeField] private DescriptiveImageBehaviour _descriptiveImage;
        [SerializeField] private TextMeshProUGUI _shownWordText;

        [Inject] protected IPracticeSettingsRepository practiceSettingsRepository;

        protected WordEntry currentWord;

        public virtual void Init(CardBehaviour cardBehaviour) { }

        public virtual void SetCurrentWord(WordEntry wordEntry)
        {
            currentWord = wordEntry;

            UpdateView();
        }

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        protected virtual void UpdateView()
        {
            _shownWordText.text = currentWord.Word.GetShownText(practiceSettingsRepository);

            if (_descriptiveImage)
                _descriptiveImage.UpdateView(currentWord.DescriptiveImage);
        }
    }
}