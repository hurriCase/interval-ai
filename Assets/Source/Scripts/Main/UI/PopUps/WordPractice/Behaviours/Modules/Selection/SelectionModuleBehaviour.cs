using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using Source.Scripts.UI.Data;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Selection
{
    internal sealed class SelectionModuleBehaviour : TransitionPracticeModuleBase<WordSelectionItem>
    {
        private const int SelectionCount = 4;

        private int _currentIndex;

        private IAnimationsConfig _animationsConfig;
        private IWordsRepository _wordsRepository;

        [Inject]
        internal void Inject(IAnimationsConfig animationsConfig, IWordsRepository wordsRepository)
        {
            _animationsConfig = animationsConfig;
            _wordsRepository = wordsRepository;
        }

        protected override async UniTask SwitchModule(ModuleType moduleType)
        {
            transitionData[_currentIndex].TransitionObject.SeActive();

            await UniTask.WaitForSeconds(_animationsConfig.SelectionTransitionDuration,
                true, cancellationToken: destroyCancellationToken);

            base.SwitchModule(moduleType).Forget();
        }

        protected override void UpdateView()
        {
            base.UpdateView();

            using var randomWords =
                _wordsRepository.GetRandomWords(currentWord, SelectionCount - 1);

            _currentIndex = Random.Range(0, SelectionCount);

            var randomWordsSpan = randomWords.Span;
            var transitionIndex = 0;

            foreach (var wordEntry in randomWordsSpan)
            {
                if (transitionIndex == _currentIndex)
                    transitionIndex++;

                InitTransitionObject(transitionIndex, wordEntry, false);
                transitionIndex++;
            }

            InitTransitionObject(_currentIndex, currentWord, true);
        }

        private void InitTransitionObject(int index, WordEntry wordEntry, bool isCorrect)
        {
            var transitionObject = transitionData[index].TransitionObject;
            transitionObject.Init(wordEntry, isCorrect);
        }
    }
}