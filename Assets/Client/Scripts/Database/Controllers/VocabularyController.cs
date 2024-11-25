using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Database;
using Client.Scripts.Database.User;
using Client.Scripts.Database.Vocabulary;

internal static class VocabularyController
{
    private static readonly IInitializable[] _entities;

    static VocabularyController()
    {
        _entities = new IInitializable[]
        {
            new WordEntity(),
            new CategoryEntity(),
            new ProgressEntity(),
            new UserEntity()
        };
    }

    internal static async Task Init()
    {
        foreach (var entity in _entities)
        {
            await entity.Init(DBController.Instance, DBController.Instance.UserId);
        }
    }
}