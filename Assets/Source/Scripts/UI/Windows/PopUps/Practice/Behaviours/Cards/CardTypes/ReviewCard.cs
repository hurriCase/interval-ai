using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Cards.Base;

namespace Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Cards.CardTypes
{
    internal sealed class ReviewCard : CardBehaviourBase
    {
        internal override void UpdateWord()
        {
            CurrentWord = VocabularyRepository.Instance.GetAvailableWord(LearningState.Repeatable);

            if (CurrentWord is null)
            {
                cardContainer.SetActive(false);
                learningCompleteBehaviour.SetActive(true);
                return;
            }

            controlButtonsBehaviour.UpdateView();

            SwitchModule(ModuleType.Input);
        }
    }
}