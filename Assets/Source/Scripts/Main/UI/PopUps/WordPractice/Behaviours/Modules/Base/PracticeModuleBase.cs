using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Shared;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    internal class PracticeModuleBase : MonoBehaviour
    {
        [SerializeField] protected DescriptiveImageBehaviour descriptiveImage;
        [SerializeField] protected TextMeshProUGUI shownWordText;
        [SerializeField] protected TransitionData[] transitionData;

        [Inject] protected IPracticeSettingsRepository practiceSettingsRepository;

        protected WordEntry currentWord;

        internal virtual void Init(CardBehaviour cardBehaviour)
        {
            foreach (var transition in transitionData)
                transition.Init(cardBehaviour, destroyCancellationToken);
        }

        internal virtual void SetCurrentWord(WordEntry wordEntry)
        {
            currentWord = wordEntry;

            UpdateView();
        }

        protected virtual void UpdateView()
        {
            if (descriptiveImage)
                descriptiveImage.UpdateView(currentWord.DescriptiveImage);

            shownWordText.text = currentWord.Word.GetShownText(practiceSettingsRepository);
        }
    }
}