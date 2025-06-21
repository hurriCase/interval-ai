using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Progress;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords
{
    internal sealed class LearningWordsScreen : ScreenBase
    {
        [SerializeField] private ProgressBehaviour _progressBehaviour;

        internal override void Init()
        {
            _progressBehaviour.Init();
        }
    }
}