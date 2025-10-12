using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours;
using Source.Scripts.Main.UI.PopUps.WordPractice;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordInfo
{
    internal sealed class WordInfoPopUp : PopUpBase
    {
        [SerializeField] private ButtonComponent _startLearningButton;
        [SerializeField] private WordInfoCardBehaviour _wordInfoCardBehaviour;

        [SerializeField] private TranslationContainer _examplesContainer;
        [SerializeField] private TranslationContainer _translationVariantsContainer;
        [SerializeField] private TranslationSetContainer _synonymsContainer;
        [SerializeField] private AnnotatedTranslationContainer _grammarContainer;

        private WordEntry _currentWordEntry;

        private ICurrentWordService _currentWordService;
        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(ICurrentWordFactory currentWordFactory, IWindowsController windowsController)
        {
            _currentWordService = currentWordFactory.GetOrCreate(PracticeState.NewWords);
            _windowsController = windowsController;
        }

        internal override void Init()
        {
            _wordInfoCardBehaviour.Init();

            _startLearningButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.StartPracticeForCurrentWord());

            _examplesContainer.Init();
            _translationVariantsContainer.Init();
            _synonymsContainer.Init();
            _grammarContainer.Init();
        }

        internal void SetParameters(WordEntry wordEntry)
        {
            _wordInfoCardBehaviour.UpdateView(wordEntry);

            _currentWordEntry = wordEntry;

            _startLearningButton.SetActive(_currentWordEntry.LearningState == LearningState.Default);

            _examplesContainer.UpdateView(wordEntry.Examples);
            _translationVariantsContainer.UpdateView(wordEntry.TranslationVariants);
            _synonymsContainer.UpdateView(wordEntry.Synonyms);
            _grammarContainer.UpdateView(wordEntry.Grammar);
        }

        private void StartPracticeForCurrentWord()
        {
            _currentWordService.SetCurrentWord(_currentWordEntry);
            _windowsController.OpenPopUp<WordPracticePopUp>();
        }
    }
}