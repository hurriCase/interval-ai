using Source.Scripts.Core.Audio.AudioRecord;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Advance;
using Source.Scripts.Core.Repositories.Words.ModuleState;
using Source.Scripts.Main.Data;
using Source.Scripts.Main.Data.CurrentWord;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress;
using Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.ExerciseStates;
using Source.Scripts.Main.UI.PopUps.Selection.Category;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete.CompleteState;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.DI
{
    internal sealed class SceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private WindowsController _windowsController;
        [SerializeField] private MenuBehaviour _menuBehaviour;
        [SerializeField] private NotificationComponent _notificationComponent;

        [SerializeField] private ProgressDescriptionsDatabase _progressDescriptionsDatabase;

        [SerializeField] private ProgressGraphSettings _progressGraphSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).AsImplementedInterfaces();
            builder.RegisterComponent(_menuBehaviour).AsImplementedInterfaces();
            builder.RegisterComponent(_notificationComponent).AsImplementedInterfaces();

            builder.RegisterComponent(_progressDescriptionsDatabase).AsImplementedInterfaces();

            builder.RegisterComponent(_progressGraphSettings).AsImplementedInterfaces();

            builder.Register<PracticeStateService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<CurrentWordFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ModuleServiceFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CompleteServiceFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WordAdvanceFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ExerciseStateFactory>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<WordsTimerService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<CategorySelectionService>(Lifetime.Singleton).AsSelf();
            builder.Register<WordCategorySelectionService>(Lifetime.Singleton).AsSelf();

#if UNITY_ANDROID && !UNITY_EDITOR
            builder.Register<AndroidSpeechRecognizer>(Lifetime.Singleton).AsImplementedInterfaces();
#else
            builder.Register<EditorSpeechRecognizer>(Lifetime.Singleton).AsImplementedInterfaces();
#endif

            builder.Register<GraphDataProcessor>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterEntryPoint<SceneEntryPoint>();
        }
    }
}