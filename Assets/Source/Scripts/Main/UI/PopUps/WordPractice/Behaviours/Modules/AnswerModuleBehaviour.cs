using Source.Scripts.Data.Repositories.Categories;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules
{
    internal sealed class AnswerModuleBehaviour : PracticeModuleBase
    {
        [SerializeField] private TextMeshProUGUI _hiddenWord;
        [SerializeField] private TextMeshProUGUI _shownExampleText;
        [SerializeField] private TextMeshProUGUI _hiddenExampleText;

        protected override void UpdateView()
        {
            base.UpdateView();

            _hiddenWord.text = currentWord.GetHiddenWord(settingsRepository);
            _shownExampleText.text = currentWord.Example.GetShownExample(settingsRepository);
            _hiddenExampleText.text = currentWord.Example.GetHiddenExample(settingsRepository);
        }
    }
}