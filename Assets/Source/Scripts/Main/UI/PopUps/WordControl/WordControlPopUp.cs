using Cysharp.Threading.Tasks;
using PrimeTween;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Selection;
using Source.Scripts.Main.UI.PopUps.Selection.Category;
using Source.Scripts.Main.UI.PopUps.WordInfo;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordControl
{
    internal sealed class WordControlPopUp : PopUpBase
    {
        [SerializeField] private ButtonComponent _showWordInfoButton;
        [SerializeField] private ButtonComponent _editButton;
        [SerializeField] private ButtonComponent _saveToCategoryButton;
        [SerializeField] private ButtonComponent _hideWordButton;

        [SerializeField] private RectTransform _selectionsContainer;
        [SerializeField] private float _selectionAnimationDuration;

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

            await AnimatePivotAsync(0f);
        }

        internal override void Init()
        {
            _showWordInfoButton.SubscribeWithHide(this, static self => self.ShowWordInfo());
            _editButton.SubscribeWithHide(this, static self => self.ShowWordInfo());
            _hideWordButton.SubscribeWithHide(this, static self => self.HideWord());
            _saveToCategoryButton.SubscribeWithHide(this, static self => self.OpenCategorySelection());
        }

        internal override async UniTask HideAsync()
        {
            await AnimatePivotAsync(1f);

            base.HideAsync().Forget();
        }

        private Tween AnimatePivotAsync(float endValue)
            => Tween.UIPivotY(_selectionsContainer, endValue, animationsConfig.SelectionSwitchDuration);

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

            var selectionPopUp = _windowsController.OpenPopUp<SelectionPopUp>();
            selectionPopUp.SetParameters(_wordCategorySelectionService);
        }
    }
}