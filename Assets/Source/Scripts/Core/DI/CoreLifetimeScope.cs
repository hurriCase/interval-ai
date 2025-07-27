using System.Collections.Generic;
using Source.Scripts.Core.DI.StartUp;
using Source.Scripts.Core.Input;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Scenes;
using Source.Scripts.Data.Repositories.Categories;
using Source.Scripts.Data.Repositories.Categories.Base;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Progress.Base;
using Source.Scripts.Data.Repositories.Progress.Tests;
using Source.Scripts.Data.Repositories.Settings;
using Source.Scripts.Data.Repositories.Settings.Base;
using Source.Scripts.Data.Repositories.Statistics;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.User.Base;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Data.Repositories.Words.Base;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Core.DI
{
    internal sealed class CoreLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<StepBase> _stepsList;

        [SerializeField] private SceneReferences _sceneReferences;

        [SerializeField] private InputActionAsset _uiInputActionAsset;

        [SerializeField] private DefaultSettingsDatabase _defaultSettingsDatabase;
        [SerializeField] private DefaultUserDataDatabase _defaultUserDataDatabase;
        [SerializeField] private DefaultCategoriesDatabase _defaultCategoriesDatabase;
        [SerializeField] private DefaultWordsDatabase _defaultWordsDatabase;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
            builder.RegisterInstance(_sceneReferences).As<ISceneReferences>();

            builder.RegisterInstance(_stepsList);

            ConfigureRepositories(builder);

            builder.Register<InputSystemUI>(Lifetime.Singleton);
            builder.Register<SwipeInputService>(Lifetime.Singleton).As<ISwipeInputService>();

            builder.Register<DateProgressHelper>(Lifetime.Singleton).As<IDateProgressHelper>();

            builder.Register<TestDataFactory>(Lifetime.Singleton).As<ITestDataFactory>();

            builder.Register<AddressablesLoader>(Lifetime.Singleton).As<IAddressablesLoader>();

            builder.RegisterEntryPoint<CoreEntryPoint>();
        }

        private void ConfigureRepositories(IContainerBuilder builder)
        {
            builder.Register<ProgressRepository>(Lifetime.Singleton).As<IProgressRepository>();

            builder.Register<StatisticsRepository>(Lifetime.Singleton).As<IStatisticsRepository>();

            builder.Register<SettingsRepository>(Lifetime.Singleton).As<ISettingsRepository>();
            builder.RegisterInstance(_defaultSettingsDatabase).As<IDefaultSettingsDatabase>();

            builder.Register<UserRepository>(Lifetime.Singleton).As<IUserRepository>();
            builder.RegisterInstance(_defaultUserDataDatabase).As<IDefaultUserDataDatabase>();

            builder.Register<WordsRepository>(Lifetime.Singleton).As<IWordsRepository>();
            builder.RegisterInstance(_defaultWordsDatabase).As<IDefaultWordsDatabase>();

            builder.Register<CategoriesRepository>(Lifetime.Singleton).As<ICategoriesRepository>();
            builder.RegisterInstance(_defaultCategoriesDatabase).As<IDefaultCategoriesDatabase>();
        }
    }
}