using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    internal class PracticeModuleBase : MonoBehaviour
    {
        [SerializeField] protected Image descriptiveImage;
        [SerializeField] protected TextMeshProUGUI shownWordText;
        [SerializeField] protected TransitionButtonData[] transitionButtons;

        [Inject] protected IUserRepository userRepository;

        protected WordEntry currentWord;

        internal virtual void Init()
        {
            foreach (var transitionButton in transitionButtons)
                transitionButton.Init(destroyCancellationToken);
        }

        internal virtual void SetCurrentWord(WordEntry wordEntry)
        {
            currentWord = wordEntry;

            UpdateView();
        }

        protected virtual void UpdateView()
        {
            if (descriptiveImage)
                descriptiveImage.sprite = currentWord.DescriptiveImage;

            shownWordText.text = currentWord.GetShownWord(userRepository);
        }
    }
}