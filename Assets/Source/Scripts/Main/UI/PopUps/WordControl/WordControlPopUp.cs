using CustomUtils.Runtime.Animations;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Selection.Category;
using Source.Scripts.Main.UI.PopUps.Selection.PopUps;
using Source.Scripts.Main.UI.PopUps.WordInfo;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordControl
{
    internal sealed class WordControlPopUp : PopUpBase
    {
        [SerializeField] private ThemeButton _showWordInfoButton;
        [SerializeField] private ThemeButton _editButton;
        [SerializeField] private ThemeButton _saveToCategoryButton;
        [SerializeField] private ThemeButton _hideWordButton;

        [SerializeField] private PivotAnimation<VisibilityState> _pivotAnimation;

        [SerializeField] private LocalizationKey _categorySelectionTitleKey;

        private WordEntry _currentWordEntry;

        private WordCategorySelectionService _wordCategorySelectionService;
        private IWindowsController _windowsController;
        private IWordStateMutator _wordStateMutator;

        [Inject]
        internal void Inject(
            WordCategorySelectionService wordCategorySelectionService,
            IWindowsController windowsController,
            IWordStateMutator wordStateMutator)
        {
            _wordCategorySelectionService = wordCategorySelectionService;
            _windowsController = windowsController;
            _wordStateMutator = wordStateMutator;
        }

        internal void SetParameters(WordEntry wordEntry)
        {
            _currentWordEntry = wordEntry;
        }

        internal override async UniTask ShowAsync()
        {
            await base.ShowAsync();

            await _pivotAnimation.PlayAnimation(VisibilityState.Visible);
        }

        internal override async UniTask HideAsync()
        {
            await _pivotAnimation.PlayAnimation(VisibilityState.Hidden);

            base.HideAsync().Forget();
        }

        internal override void Init()
        {
            _showWordInfoButton.SubscribeWithHide(this, static self => self.ShowWordInfo());
            _editButton.SubscribeWithHide(this, static self => self.ShowWordInfo());
            _hideWordButton.SubscribeWithHide(this, static self => self.HideWord());
            _saveToCategoryButton.SubscribeWithHide(this, static self => self.OpenCategorySelection());
        }

        private void ShowWordInfo()
        {
            var wordInfoPopUp = _windowsController.OpenPopUp<WordInfoPopUp>();
            wordInfoPopUp.SetParameters(_currentWordEntry);
        }

        private void HideWord()
        {
            _wordStateMutator.HideWord(_currentWordEntry);
        }

        private void OpenCategorySelection()
        {
            _wordCategorySelectionService.UpdateWord(_currentWordEntry);
            _wordCategorySelectionService.UpdateData();

            var selectionPopUp = _windowsController.OpenPopUp<EnumSelectionPopUp>();
            selectionPopUp.SetParameters(_wordCategorySelectionService, _categorySelectionTitleKey.GetLocalization());
        }
    }
}