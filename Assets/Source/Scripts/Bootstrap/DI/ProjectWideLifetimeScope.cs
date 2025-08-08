using System.Collections.Generic;
using Source.Scripts.Bootstrap.Core;
using Source.Scripts.Core.Audio;
using Source.Scripts.Core.Importer;
using Source.Scripts.Core.Input;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Base.DefaultConfig;
using Source.Scripts.Core.Repositories.Base.Id;
using Source.Scripts.Core.Repositories.Base.Tests;
using Source.Scripts.Core.Repositories.Base.Tests.Base;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Progress;
using Source.Scripts.Core.Repositories.Settings;
using Source.Scripts.Core.Repositories.Statistics;
using Source.Scripts.Core.Repositories.User;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Advance;
using Source.Scripts.Core.Repositories.Words.Timer;
using Source.Scripts.Core.Scenes;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Bootstrap.DI
{
    internal sealed class ProjectWideLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<StepBase> _stepsList;

        [SerializeField] private SceneReferences _sceneReferences;
        [SerializeField] private InputActionAsset _uiInputActionAsset;

        [SerializeField] private LocalizationKeysDatabase _localizationKeysDatabase;

        [SerializeField] private TestDataConfig _testDataConfig;
        [SerializeField] private DefaultSettingsConfig _defaultSettingsConfig;
        [SerializeField] private DefaultUserDataConfig _defaultUserDataConfig;
        [SerializeField] private DefaultWordsDatabase _defaultWordsDatabase;
        [SerializeField] private DefaultCategoriesDatabase _defaultCategoriesDatabase;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_sceneReferences).AsImplementedInterfaces();

            builder.RegisterInstance(_stepsList);

            builder.Register<InputSystemUI>(Lifetime.Singleton);
            builder.Register<SwipeInputService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<AddressablesLoader>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<AudioHandlerProvider>(Lifetime.Singleton).As<IAudioHandlerProvider>();

            builder.RegisterComponent(_localizationKeysDatabase).As<ILocalizationKeysDatabase>();

            RegisterRepositories(builder);
            RegisterCSV(builder);

            builder.RegisterEntryPoint<EntryPoint>();
        }

        private void RegisterRepositories(IContainerBuilder builder)
        {
            builder.Register<TestDataFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_testDataConfig).AsImplementedInterfaces();

            builder.Register<StatisticsRepository>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<SettingsRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultSettingsConfig).AsImplementedInterfaces();

            builder.Register<ProgressRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<DateProgressService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<UserRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultUserDataConfig).AsImplementedInterfaces();

            RegisterWordsRepository(builder);

            builder.Register<IdHandler<CategoryEntry>>(Lifetime.Singleton).As<IIdHandler<CategoryEntry>>();
            builder.Register<CategoriesRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultCategoriesDatabase).As<IDefaultDatabase>().AsSelf();
        }

        private void RegisterWordsRepository(IContainerBuilder builder)
        {
            builder.Register<IdHandler<WordEntry>>(Lifetime.Singleton).As<IIdHandler<WordEntry>>();
            builder.Register<WordsRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WordsTimerService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WordAdvanceService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultWordsDatabase).As<IDefaultDatabase>().AsSelf();
        }

        private void RegisterCSV(IContainerBuilder builder)
        {
            builder.Register<CSVMapper>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CSVReader>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}