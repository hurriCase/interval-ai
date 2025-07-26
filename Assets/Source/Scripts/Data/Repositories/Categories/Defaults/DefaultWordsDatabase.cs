using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Categories.Base;
using UnityEngine;
using WordEntry = Source.Scripts.Data.Repositories.Words.WordEntry;

namespace Source.Scripts.Data.Repositories.Categories.Defaults
{
    internal sealed class DefaultWordsDatabase : ScriptableObject, IDefaultWordsDatabase
    {
        [field: SerializeField] public List<WordEntry> WordEntries { get; private set; }
    }
}