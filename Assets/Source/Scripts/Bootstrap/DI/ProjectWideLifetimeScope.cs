using System.Collections.Generic;
using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Scenes;
using Source.Scripts.Bootstrap.Core;
using Source.Scripts.Core.ApiHelper;
using Source.Scripts.Core.Audio.Sounds;
using Source.Scripts.Core.Audio.TextToSpeech;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Input;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.References;
using Source.Scripts.Core.Repositories.Base.Id;
using Source.Scripts.Core.Repositories.Base.Tests;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Core.Repositories.Progress;
using Source.Scripts.Core.Repositories.Settings;
using Source.Scripts.Core.Repositories.Settings.Repositories;
using Source.Scripts.Core.Repositories.Statistics;
using Source.Scripts.Core.Repositories.User;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.UI.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Bootstrap.DI
{
    internal sealed class ProjectWideLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<StepBase> _stepsList;

        [SerializeField] private SceneReferences _sceneReferences;

        [SerializeField] private SwipeConfig _swipeConfig;

        [SerializeField] private SpriteReferences _spriteReferences;

        [SerializeField] private GoogleTextToSpeechConfig _googleTextToSpeechConfig;
        [SerializeField] private AnimationsConfig _animationsConfig;
        [SerializeField] private TestConfig _testConfig;
        [SerializeField] private AppConfig _appConfig;

        [SerializeField] private LocalizationKeysDatabase _localizationKeysDatabase;
        [SerializeField] private LocalizationDatabase _localizationDatabase;

        [SerializeField] private DefaultCategoriesDatabase _defaultCategoriesDatabase;
        [SerializeField] private DefaultSettingsConfig _defaultSettingsConfig;
        [SerializeField] private DefaultUserDataConfig _defaultUserDataConfig;
        [SerializeField] private DefaultWordsDatabase _defaultWordsDatabase;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneTransitionController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_sceneReferences).AsImplementedInterfaces();

            builder.RegisterInstance(_stepsList);

            builder.Register<AddressablesLoader>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<AudioHandlerProvider>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<ApiHelper>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_googleTextToSpeechConfig).AsImplementedInterfaces();
            builder.Register<GoogleTextToSpeech>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterInstance(_spriteReferences).AsImplementedInterfaces();

            builder.RegisterInstance(_animationsConfig).AsImplementedInterfaces();
            builder.RegisterInstance(_testConfig).AsImplementedInterfaces();
            builder.RegisterInstance(_appConfig).AsImplementedInterfaces();

            builder.RegisterComponent(_localizationKeysDatabase).AsImplementedInterfaces();
            builder.RegisterComponent(_localizationDatabase).AsImplementedInterfaces();

            RegisterInput(builder);
            RegisterRepositories(builder);

            builder.RegisterEntryPoint<EntryPoint>();
        }

        private void RegisterInput(IContainerBuilder builder)
        {
            builder.Register<InputSystemUI>(Lifetime.Singleton);
            builder.Register<SwipeInputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_swipeConfig).AsImplementedInterfaces();
        }

        private void RegisterRepositories(IContainerBuilder builder)
        {
            builder.Register<TestDataFactory>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<StatisticsRepository>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<UISettingsRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PracticeSettingsRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LanguageSettingsRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GenerationSettingsRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultSettingsConfig).AsImplementedInterfaces();

            builder.Register<ProgressRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<DateProgressService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<UserRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultUserDataConfig).AsImplementedInterfaces();

            RegisterCategoriesRepository(builder);
            RegisterWordsRepository(builder);
        }

        private void RegisterCategoriesRepository(IContainerBuilder builder)
        {
            builder.Register<IdHandler<CategoryEntry>>(Lifetime.Singleton).As<IIdHandler<CategoryEntry>>();
            builder.Register<CategoriesRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CategoryEntry.CategoryStateMutator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CategoryEntry.CategoryConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultCategoriesDatabase)
                .As<DefaultCategoriesDatabase>()
                .AsImplementedInterfaces();
        }

        private void RegisterWordsRepository(IContainerBuilder builder)
        {
            builder.Register<IdHandler<WordEntry>>(Lifetime.Singleton).As<IIdHandler<WordEntry>>();
            builder.Register<WordsRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WordEntry.WordStateMutator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WordEntry.WordConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultWordsDatabase)
                .As<DefaultWordsDatabase>()
                .AsImplementedInterfaces();
        }
    }
}