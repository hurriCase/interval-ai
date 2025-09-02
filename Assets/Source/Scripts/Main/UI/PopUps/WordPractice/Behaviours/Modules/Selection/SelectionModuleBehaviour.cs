using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Selection;
using Source.Scripts.UI.Data;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules
{
    internal sealed class SelectionModuleBehaviour : TransitionPracticeModuleBase<SelectionItem>
    {
        [SerializeField] private float _incorrectBorderRatio;

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
            transitionData[_currentIndex].TransitionObject.Checkbox.isOn = true;

            await UniTask.WaitForSeconds(_animationsConfig.SelectionTransitionDuration,
                true, cancellationToken: destroyCancellationToken);

            base.SwitchModule(moduleType).Forget();
        }

        protected override void UpdateView()
        {
            base.UpdateView();

            var randomWords = _wordsRepository.GetRandomWords(currentWord, SelectionCount - 1);
            _currentIndex = Random.Range(0, SelectionCount);

            var index = -1;
            foreach (var wordEntry in randomWords)
            {
                index++;
                if (index == _currentIndex)
                {
                    index++;
                    continue;
                }

                InitTransitionObject(index, wordEntry, false);
            }

            InitTransitionObject(_currentIndex, currentWord, true);
        }

        private void InitTransitionObject(int index, WordEntry wordEntry, bool isCorrect)
        {
            var transitionObject = transitionData[index].TransitionObject;
            transitionObject.Checkbox.Text.text = wordEntry.Word.GetHiddenText(practiceSettingsRepository);
            transitionObject.Image.BorderRatio = isCorrect ? 0 : _incorrectBorderRatio;
        }
    }
}