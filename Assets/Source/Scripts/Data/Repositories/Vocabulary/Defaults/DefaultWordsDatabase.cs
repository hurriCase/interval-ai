using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Vocabulary.Base;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary.Defaults
{
    internal sealed class DefaultWordsDatabase : ScriptableObject, IDefaultWordsDatabase
    {
        [field: SerializeField] public List<WordEntry> WordEntries { get; private set; }
    }
}