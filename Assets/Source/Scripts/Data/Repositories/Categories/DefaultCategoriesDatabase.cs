using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Categories.Base;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Categories
{
    internal sealed class DefaultCategoriesDatabase : ScriptableObject, IDefaultCategoriesDatabase
    {
        [field: SerializeField] public List<CategoryEntry> Categories { get; private set; }
    }
}