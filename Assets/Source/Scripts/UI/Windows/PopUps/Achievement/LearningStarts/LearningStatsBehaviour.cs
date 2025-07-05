using System.Linq;
using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Achievements;
using Source.Scripts.UI.Windows.Shared;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement.LearningStarts
{
    internal sealed class LearningStatsBehaviour : MonoBehaviour
    {
        [SerializeField] private WeekDaysBehaviour _weekDaysBehaviour;
        [SerializeField] private DateIdentifierMapping _dateIdentifierMapping;
        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] private ProgressItem _totalProgressItem;

        [SerializeField] private ProgressDescriptionItem[] _progressDescriptionItems = new ProgressDescriptionItem[4];

        internal void Init()
        {
            _weekDaysBehaviour.Init();

            ProgressRepository.Instance.ProgressEntry.Subscribe(this,
                    static (entry, behaviour) => behaviour.UpdateProgress(entry.StateCounts))
                .RegisterTo(destroyCancellationToken);
        }

        private void UpdateProgress(int[] stateCounts)
        {
            var totalWords = stateCounts.Sum().ToString();
            _totalProgressItem.Init(stateCounts, totalWords, _dateIdentifierMapping, _progressColorMapping);

            foreach (var progressDescriptionItem in _progressDescriptionItems)
                progressDescriptionItem.Init(stateCounts, _progressColorMapping);
        }
    }
}