using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    internal class PracticeModule : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI shownWordText;

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
            shownWordText.text = currentWord.Word.GetShownText(practiceSettingsRepository);
        }
    }
}