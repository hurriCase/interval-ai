using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Answer
{
    internal sealed class AnswerModuleBehaviour : PracticeModuleBase
    {
        [SerializeField] private TextMeshProUGUI _hiddenWord;

        [SerializeField] private ExampleItem _exampleItem;
        [SerializeField] private RectTransform _exampleContainer;
        [SerializeField] private AspectRatioFitter _spacing;
        [SerializeField] private float _exampleSpacingRatio;

        private UIPool<ExampleItem> _examplesPool;

        internal override void Init(CardBehaviour cardBehaviour)
        {
            base.Init(cardBehaviour);

            _examplesPool = new UIPool<ExampleItem>(_exampleItem, _exampleContainer, _spacing, _exampleSpacingRatio);
        }

        protected override void UpdateView()
        {
            base.UpdateView();

            _hiddenWord.text = currentWord.Word.GetHiddenText(practiceSettingsRepository);

            TryCreateExamples();
        }

        private void TryCreateExamples()
        {
            var examples = currentWord.Examples;
            if (examples is null || examples.Count == 0)
                return;

            var examplesCount = examples.Count;
            _examplesPool.EnsureCount(examplesCount);

            for (var i = 0; i < examplesCount; i++)
            {
                var example = examples[i];
                var item = _examplesPool.PooledItems[i];

                item.AccordionComponent.Init();
                item.ShownExampleText.text = example.GetShownText(practiceSettingsRepository);
                item.HiddenExampleText.text = example.GetHiddenText(practiceSettingsRepository);
            }
        }
    }
}