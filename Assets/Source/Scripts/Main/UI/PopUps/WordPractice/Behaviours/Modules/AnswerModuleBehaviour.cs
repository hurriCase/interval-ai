using Source.Scripts.Data.Repositories.Categories;
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

            _hiddenWord.text = currentWord.GetHiddenWord(userRepository);
            _shownExampleText.text = currentWord.Example.GetShownExample(userRepository);
            _hiddenExampleText.text = currentWord.Example.GetHiddenExample(userRepository);
        }
    }
}