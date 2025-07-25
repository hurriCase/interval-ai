using System.Collections.Generic;
using Source.Scripts.Core.Scenes;
using Source.Scripts.Core.StartUp;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Progress.Tests;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Vocabulary;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Core
{
    internal sealed class CoreLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<StepBase> _stepsList;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();

            builder.RegisterInstance(_stepsList);

            builder.Register<ProgressRepository>(Lifetime.Singleton).As<IProgressRepository>();
            builder.Register<VocabularyRepository>(Lifetime.Singleton).As<IVocabularyRepository>();
            builder.Register<UserRepository>(Lifetime.Singleton).As<IUserRepository>();

            builder.Register<DateProgressHelper>(Lifetime.Singleton).As<IDateProgressHelper>();

            builder.Register<TestDataFactory>(Lifetime.Singleton).As<ITestDataFactory>();

            builder.RegisterEntryPoint<CoreEntryPoint>();
        }
    }
}