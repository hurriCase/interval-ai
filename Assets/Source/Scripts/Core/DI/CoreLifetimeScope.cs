using System.Collections.Generic;
using Source.Scripts.Core.DI.StartUp;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Scenes;
using Source.Scripts.Data.Repositories.Categories;
using Source.Scripts.Data.Repositories.Categories.Base;
using Source.Scripts.Data.Repositories.Categories.Defaults;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Progress.Base;
using Source.Scripts.Data.Repositories.Progress.Tests;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.User.Base;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Data.Repositories.Words.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Core.DI
{
    internal sealed class CoreLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<StepBase> _stepsList;

        [SerializeField] private SceneReferences _sceneReferences;

        [SerializeField] private DefaultUserDataDatabase _defaultUserDataDatabase;
        [SerializeField] private DefaultCategoriesDatabase _defaultCategoriesDatabase;
        [SerializeField] private DefaultWordsDatabase _defaultWordsDatabase;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
            builder.RegisterInstance(_sceneReferences).As<ISceneReferences>();

            builder.RegisterInstance(_stepsList);

            builder.Register<ProgressRepository>(Lifetime.Singleton).As<IProgressRepository>();

            builder.Register<UserRepository>(Lifetime.Singleton).As<IUserRepository>();
            builder.RegisterInstance(_defaultUserDataDatabase).As<IDefaultUserDataDatabase>();

            builder.Register<WordsRepository>(Lifetime.Singleton).As<IWordsRepository>();
            builder.RegisterInstance(_defaultWordsDatabase).As<IDefaultWordsDatabase>();

            builder.Register<CategoriesRepository>(Lifetime.Singleton).As<ICategoriesRepository>();
            builder.RegisterInstance(_defaultCategoriesDatabase).As<IDefaultCategoriesDatabase>();

            builder.Register<DateProgressHelper>(Lifetime.Singleton).As<IDateProgressHelper>();

            builder.Register<TestDataFactory>(Lifetime.Singleton).As<ITestDataFactory>();

            builder.Register<AddressablesLoader>(Lifetime.Singleton).As<IAddressablesLoader>();

            builder.RegisterEntryPoint<CoreEntryPoint>();
        }
    }
}