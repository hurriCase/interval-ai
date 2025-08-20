using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordInfo;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    internal class PracticeModuleBase : MonoBehaviour
    {
        [SerializeField] protected DescriptiveImageBehaviour descriptiveImage;
        [SerializeField] protected TextMeshProUGUI shownWordText;
        [SerializeField] protected TransitionButtonData[] transitionButtons;

        [Inject] protected ISettingsRepository settingsRepository;
        [Inject] protected IAddressablesLoader addressablesLoader;

        protected WordEntry currentWord;

        internal virtual void Init(CardBehaviour cardBehaviour)
        {
            foreach (var transitionButton in transitionButtons)
                transitionButton.Init(cardBehaviour, destroyCancellationToken);
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

            shownWordText.text = currentWord.GetShownWord(settingsRepository);
        }
    }
}