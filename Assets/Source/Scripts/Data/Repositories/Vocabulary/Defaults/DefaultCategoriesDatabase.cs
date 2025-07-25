using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary.Defaults
{
    internal sealed class DefaultCategoriesDatabase : ScriptableObject, IDefaultCategoriesDatabase
    {
        [field: SerializeField] public List<CategoryEntry> Categories { get; private set; }
    }
}