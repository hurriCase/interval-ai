using System;
using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.Core.Localization;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.Base;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.CardTypes
{
    internal sealed class ReviewCard : CardBehaviourBase
    {
        internal override void OnInit()
        {
            base.OnInit();

            learningCompleteBehaviour.Init(PracticeState.Review);

            wordsRepository.OnAvailabilityTimeUpdate
                .Where(cooldown => cooldown.State == LearningState.Repeatable)
                .Where(cooldown => DateTime.Now >= cooldown.CurrentTime)
                .Subscribe(this, static (_, card) => card.UpdateWord())
                .RegisterTo(destroyCancellationToken);
        }

        internal override void UpdateWord()
        {
            CurrentWord = wordsRepository.GetAvailableWord(LearningState.Repeatable);

            base.UpdateWord();
        }

        internal override void UpdateView()
        {
            base.UpdateView();

            SwitchModule(ModuleType.Input);

            var wordsCount = progressRepository.ReviewCount;
            var localizationKey = localizationKeysDatabase.GetLearnedCountLocalization(wordsCount);

            learnedText.text = string.Format(LocalizationController.Localize(localizationKey), wordsCount);
        }
    }
}