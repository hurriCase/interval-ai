using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Categories.Base;
using UnityEngine;
using CategoryEntry = Source.Scripts.Data.Repositories.Categories.CategoryEntry;

namespace Source.Scripts.Data.Repositories.Categories.Defaults
{
    internal sealed class DefaultCategoriesDatabase : ScriptableObject, IDefaultCategoriesDatabase
    {
        [field: SerializeField] public List<CategoryEntry> Categories { get; private set; }
    }
}