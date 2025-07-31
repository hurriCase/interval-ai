using Source.Scripts.Core.DI.Repositories.Categories;
using Source.Scripts.Core.DI.Repositories.Progress.Base;
using Source.Scripts.Core.DI.Repositories.Settings.Base;
using Source.Scripts.Core.DI.Repositories.Statistics;
using Source.Scripts.Core.DI.Repositories.User.Base;
using Source.Scripts.Core.DI.Repositories.Words.Base;
using Source.Scripts.Data.Repositories.Categories;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Settings;
using Source.Scripts.Data.Repositories.Statistics;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Data.Repositories.Words.Advance;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Data.DI
{
    internal sealed class DataLifetimeScope : LifetimeScope
    {
        [SerializeField] private DefaultSettingsDatabase _defaultSettingsDatabase;
        [SerializeField] private DefaultUserDataDatabase _defaultUserDataDatabase;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<DateProgressHelper>(Lifetime.Singleton).As<IDateProgressHelper>();

            builder.Register<TestDataFactory>(Lifetime.Singleton).As<ITestDataFactory>();

            builder.Register<ProgressRepository>(Lifetime.Singleton).As<IProgressRepository>();

            builder.Register<StatisticsRepository>(Lifetime.Singleton).As<IStatisticsRepository>();

            builder.Register<SettingsRepository>(Lifetime.Singleton).As<ISettingsRepository>();
            builder.RegisterInstance(_defaultSettingsDatabase).As<IDefaultSettingsDatabase>();

            builder.Register<UserRepository>(Lifetime.Singleton).As<IUserRepository>();
            builder.RegisterInstance(_defaultUserDataDatabase).As<IDefaultUserDataDatabase>();

            builder.Register<WordsRepository>(Lifetime.Singleton).As<IWordsRepository>();
            builder.Register<WordAdvanceHelper>(Lifetime.Singleton).As<IWordAdvanceHelper>();

            builder.Register<CategoriesRepository>(Lifetime.Singleton).As<ICategoriesRepository>();
        }
    }
}