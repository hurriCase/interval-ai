using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Loader;
using Source.Scripts.Data.Repositories.Categories;
using Source.Scripts.Data.Repositories.Settings;
using Source.Scripts.Data.Repositories.Settings.Base;
using Source.Scripts.Data.Repositories.User.Base;
using Source.Scripts.Data.Repositories.Words;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using WordEntry = Source.Scripts.Data.Repositories.Words.Data.WordEntry;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    internal class PracticeModuleBase : MonoBehaviour
    {
        [SerializeField] protected Image descriptiveImage;
        [SerializeField] protected TextMeshProUGUI shownWordText;
        [SerializeField] protected TransitionButtonData[] transitionButtons;

        [Inject] protected ISettingsRepository settingsRepository;
        [Inject] protected IAddressablesLoader addressablesLoader;

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
            SetDescriptiveImage().Forget();

            shownWordText.text = currentWord.GetShownWord(settingsRepository);
        }

        private async UniTask SetDescriptiveImage()
        {
            if (!descriptiveImage)
                return;

            if (currentWord.DescriptiveImage.IsValid is false)
            {
                descriptiveImage.SetActive(false);
                return;
            }

            descriptiveImage.sprite = await addressablesLoader
                .LoadAsync<Sprite>(currentWord.DescriptiveImage.AssetGUID, destroyCancellationToken);
        }
    }
}