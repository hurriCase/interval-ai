using System;
using R3;
using Source.Scripts.Core.Localization;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Data.Repositories.Words.Base;
using Source.Scripts.UI.Windows.Base;
using VContainer;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.LearningComplete
{
    internal sealed class ReviewCompleteBehaviour : LearningCompleteBehaviourBase
    {
        [Inject] private IWordsRepository _wordsRepository;

        protected override void OnInit()
        {
            _wordsRepository.OnAvailabilityTimeUpdate
                .Where(cooldownByLearningState => cooldownByLearningState.State == LearningState.Repeatable)
                .Subscribe(this, static (cooldownByLearningState, card) => card.SetState(
                    CompleteState.Complete,
                    cooldownByLearningState.CurrentTime.ToShortTimeString())
                )
                .RegisterTo(destroyCancellationToken);

            exitButton.OnClickAsObservable()
                .Subscribe(windowsController,
                    static (_, controller) => controller.OpenScreenByType(ScreenType.LearningWords))
                .RegisterTo(destroyCancellationToken);
        }

        //TODO:<Dmitriy.Sukharev> refactor, or maybe just use built-in
        private static string FormatTimeUntil(DateTime targetTime)
        {
            var timeSpan = targetTime - DateTime.Now;

            switch (timeSpan.TotalSeconds)
            {
                case <= 0:
                    return "Available now";
                case < 60:
                    return $"{(int)timeSpan.TotalSeconds}s";
            }

            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes}m";

            if (timeSpan.TotalHours < 24)
            {
                var hours = (int)timeSpan.TotalHours;
                var minutes = timeSpan.Minutes;

                return minutes > 5 ? $"{hours}h {minutes}m" : $"{hours}h";
            }

            if ((timeSpan.TotalDays < 7) is false)
                return $"{(int)timeSpan.TotalDays}d";
            {
                var days = (int)timeSpan.TotalDays;
                var hours = timeSpan.Hours;

                return hours > 2 ? $"{days}d {hours}h" : $"{days}d";
            }
        }
    }
}