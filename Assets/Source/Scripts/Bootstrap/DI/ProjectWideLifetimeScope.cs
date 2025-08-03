using System.Collections.Generic;
using Source.Scripts.Bootstrap.Core;
using Source.Scripts.Core.Audio;
using Source.Scripts.Core.Importer;
using Source.Scripts.Core.Input;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Repositories;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Scenes;
using Source.Scripts.Data.Repositories;
using Source.Scripts.Data.Repositories.Categories;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Settings;
using Source.Scripts.Data.Repositories.Statistics;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Data.Repositories.Words.Advance;
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

        [SerializeField] private DefaultSettingsDatabase _defaultSettingsDatabase;
        [SerializeField] private DefaultUserDataDatabase _defaultUserDataDatabase;
        [SerializeField] private DefaultWordsConfig _defaultWordsConfig;
        [SerializeField] private DefaultCategoriesConfig _defaultCategoriesConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_sceneReferences).AsImplementedInterfaces();

            builder.RegisterInstance(_stepsList);

            builder.Register<InputSystemUI>(Lifetime.Singleton);
            builder.Register<SwipeInputService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<AddressablesLoader>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<AudioHandlerProvider>(Lifetime.Singleton).As<IAudioHandlerProvider>();

            RegisterRepositories(builder);
            RegisterCSV(builder);

            builder.RegisterEntryPoint<EntryPoint>();
        }

        private void RegisterRepositories(IContainerBuilder builder)
        {
            builder.Register<TestDataFactory>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<StatisticsRepository>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<SettingsRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultSettingsDatabase).AsImplementedInterfaces();

            builder.Register<ProgressRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<DateProgressHelper>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<UserRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultUserDataDatabase).AsImplementedInterfaces();

            builder.Register<IdHandler<WordEntry>>(Lifetime.Singleton).As<IIdHandler<WordEntry>>();
            builder.Register<WordsRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WordAdvanceHelper>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultWordsConfig).As<IDefaultConfig>().AsSelf();

            builder.Register<IdHandler<CategoryEntry>>(Lifetime.Singleton).As<IIdHandler<CategoryEntry>>();
            builder.Register<CategoriesRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultCategoriesConfig).As<IDefaultConfig>().AsSelf();
        }

        private void RegisterCSV(IContainerBuilder builder)
        {
            builder.Register<CSVMapper>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CSVReader>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}