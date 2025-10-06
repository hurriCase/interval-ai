using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Base.Id;
using Source.Scripts.Core.Repositories.Exercises.Exercise;

namespace Source.Scripts.Core.Repositories.Exercises
{
    internal sealed class ExercisesRepository : IRepository, IDisposable, IExercisesRepository
    {
        public ReadOnlyReactiveProperty<EnumArray<ExerciseType, Dictionary<int, ExerciseEntry>>> Exercises =>
            _exercises.Property;

        private readonly PersistentReactiveProperty<EnumArray<ExerciseType, Dictionary<int, ExerciseEntry>>>
            _exercises = new();

        private readonly DefaultSentencesDatabase _defaultSentencesDatabase;
        private readonly DefaultTextsDatabase _defaultTextsDatabase;
        private readonly IIdHandler<ExerciseEntry> _idHandler;

        private DisposableBag _disposable;

        internal ExercisesRepository(
            DefaultSentencesDatabase defaultSentencesDatabase,
            DefaultTextsDatabase defaultTextsDatabase,
            IIdHandler<ExerciseEntry> idHandler)
        {
            _defaultSentencesDatabase = defaultSentencesDatabase;
            _defaultTextsDatabase = defaultTextsDatabase;
            _idHandler = idHandler;
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            await _idHandler.InitAsync(token);

            await _exercises.InitAsync(PersistentKeys.ExercisesKey, token, CrateDefaultExercises());
        }

        private EnumArray<ExerciseType, Dictionary<int, ExerciseEntry>> CrateDefaultExercises()
        {
            var exercises = new EnumArray<ExerciseType, Dictionary<int, ExerciseEntry>>(EnumMode.SkipFirst)
            {
                [ExerciseType.Sentences] = _idHandler.GenerateWithIds(_defaultSentencesDatabase.Defaults),
                [ExerciseType.Texts] = _idHandler.GenerateWithIds(_defaultTextsDatabase.Defaults)
            };

            return exercises;
        }

        public void RemoveExercise(ExerciseType exerciseType, int exerciseId)
        {
            _exercises.Value[exerciseType].Remove(exerciseId);
        }

        public void Dispose()
        {
            _exercises.Dispose();
        }
    }
}