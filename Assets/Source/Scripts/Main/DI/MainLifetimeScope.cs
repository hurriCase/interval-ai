using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Advance;
using Source.Scripts.Core.Repositories.Words.ModuleState;
using Source.Scripts.Main.Data;
using Source.Scripts.Main.Data.CurrentWord;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress;
using Source.Scripts.Main.UI.PopUps.Selection.Category;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete.CompleteState;
using Source.Scripts.UI.Behaviours.DateLabel.Range;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.DI
{
    internal sealed class MainLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainWindowsController _windowsController;
        [SerializeField] private MenuBehaviour _menuBehaviour;

        [SerializeField] private ProgressDescriptionsDatabase _progressDescriptionsDatabase;

        [SerializeField] private ProgressGraphSettings _progressGraphSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).AsImplementedInterfaces();
            builder.RegisterComponent(_menuBehaviour).AsImplementedInterfaces();

            builder.RegisterComponent(_progressDescriptionsDatabase).AsImplementedInterfaces();

            builder.RegisterComponent(_progressGraphSettings).AsImplementedInterfaces();

            builder.Register<PracticeStateService>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.Register<MainCurrentWordFactory>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<ModuleServiceFactory>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<CompleteServiceFactory>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<WordAdvanceFactory>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<WordsTimerService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<CategorySelectionService>(Lifetime.Scoped).AsSelf();
            builder.Register<WordCategorySelectionService>(Lifetime.Scoped).AsSelf();

            builder.Register<GraphDataProcessor>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<DateRangeCalculator>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterEntryPoint<MainEntryPoint>();
        }
    }
}