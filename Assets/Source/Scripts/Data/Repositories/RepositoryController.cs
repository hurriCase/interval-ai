using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Vocabulary;
using UnityEngine;

namespace Source.Scripts.Data.Repositories
{
    internal static class RepositoryController
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            ProgressRepository.Instance.Init();

            Application.quitting += OnApplicationQuitting;
        }

        private static void OnApplicationQuitting()
        {
            ProgressRepository.Instance.Dispose();
            VocabularyRepository.Instance.Dispose();
            UserRepository.Instance.Dispose();

            Application.quitting -= OnApplicationQuitting;
        }
    }
}