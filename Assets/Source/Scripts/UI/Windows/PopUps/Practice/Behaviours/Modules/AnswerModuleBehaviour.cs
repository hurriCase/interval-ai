using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Modules.Base;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Modules
{
    internal sealed class AnswerModuleBehaviour : PracticeModuleBase
    {
        [SerializeField] private TextMeshProUGUI _hiddenWord;
        [SerializeField] private TextMeshProUGUI _shownExampleText;
        [SerializeField] private TextMeshProUGUI _hiddenExampleText;

        protected override void UpdateView()
        {
            base.UpdateView();

            _hiddenWord.text = currentWord.HiddenWord;
            _shownExampleText.text = currentWord.Example.ShownExample;
            _hiddenExampleText.text = currentWord.Example.HiddeExample;
        }
    }
}