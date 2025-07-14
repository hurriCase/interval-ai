using Source.Scripts.UI.Selectables;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Practice
{
    internal sealed class WordPracticePopUp : PopUpBase
    {
        [SerializeField] private WordCardBehaviour _wordCardBehaviour;

        [SerializeField] private ThemeToggle _learningToggle;
        [SerializeField] private ThemeToggle _repetitionToggle;

        internal override void Init()
        {
            _wordCardBehaviour.Init(_learningToggle, _repetitionToggle);
        }
    }
}