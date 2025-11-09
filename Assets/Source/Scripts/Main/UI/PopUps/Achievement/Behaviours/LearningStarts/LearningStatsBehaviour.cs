using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions.Observables;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress;
using Source.Scripts.Main.UI.Shared.Progress;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts
{
    internal sealed class LearningStatsBehaviour : MonoBehaviour
    {
        [SerializeField] private ProgressGraphBehaviour _progressGraphBehaviour;
        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] private ProgressItem _totalProgressItem;

        [SerializeField] private EnumArray<LearningState, ProgressDescriptionItem> _progressDescriptionItems;

        private IProgressRepository _progressRepository;

        [Inject]
        internal void Inject(IProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;
        }

        internal void Init()
        {
            _progressGraphBehaviour.Init();

            _progressRepository.TotalCountByState
                .SubscribeUntilDestroy(this, static (totalCountByState, self) => self.UpdateProgress(totalCountByState));
        }

        private void UpdateProgress(EnumArray<LearningState, int> totalCountByState)
        {
            var totalWords = totalCountByState.Entries.Sum(static entry => entry.Value).ToString();
            _totalProgressItem.Init(totalCountByState, totalWords);

            foreach (var (learningState, progressItem) in _progressDescriptionItems.AsTuples())
            {
                if (learningState != LearningState.Default)
                    progressItem.Init(learningState, totalCountByState[learningState], _progressColorMapping);
            }
        }
    }
}