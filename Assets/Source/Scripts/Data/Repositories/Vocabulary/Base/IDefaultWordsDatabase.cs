using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Vocabulary.Base
{
    internal interface IDefaultWordsDatabase
    {
        List<WordEntry> WordEntries { get; }
    }
}