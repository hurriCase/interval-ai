using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Words.Base;
using Source.Scripts.Data.Repositories.Words.Data;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Words
{
    internal sealed class DefaultWordsDatabase : ScriptableObject, IDefaultWordsDatabase
    {
        [field: SerializeField] public List<WordEntry> WordEntries { get; private set; }
    }
}