using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Localization;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.Base;

namespace Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.CardTypes
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

            base.UpdateWord();
        }

        internal override void UpdateView()
        {
            base.UpdateView();

            SwitchModule(ModuleType.Input);

            var wordsCount = ProgressRepository.Instance.ReviewCount;
            var localizationKey = LocalizationKeysDatabase.Instance.GetLearnedCountLocalization(wordsCount);

            learnedText.text = string.Format(LocalizationController.Localize(localizationKey), wordsCount);
        }
    }
}