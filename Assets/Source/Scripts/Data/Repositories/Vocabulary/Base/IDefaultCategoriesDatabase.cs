using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Vocabulary
{
    internal interface IDefaultCategoriesDatabase
    {
        List<CategoryEntry> Categories { get; }
    }
}