using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Localization.Translator.Translations;
using Source.Scripts.Core.Others;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Answer
{
    internal sealed class AnswerModuleBehaviour : PracticeModule
    {
        [SerializeField] private TextMeshProUGUI _hiddenWord;

        [SerializeField] private ExampleItem _exampleItem;
        [SerializeField] private RectTransform _exampleContainer;

        private UIPoolBase<ExampleItem> _examplesPoolBase;

        public override void Init(PracticeState practiceState)
        {
            _examplesPoolBase = new UIPoolBase<ExampleItem>(_exampleItem, _exampleContainer);
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
            _examplesPoolBase.EnsureCount(examplesCount);

            for (var i = 0; i < examplesCount; i++)
            {
                var example = examples[i];
                var item = _examplesPoolBase.PooledItems[i];

                item.ShownExampleText.text = example.GetShownText(practiceSettingsRepository);
                item.HiddenExampleText.text = example.GetHiddenText(practiceSettingsRepository);
            }
        }
    }
}