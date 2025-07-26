using System.Collections.Generic;
using WordEntry = Source.Scripts.Data.Repositories.Words.WordEntry;

namespace Source.Scripts.Data.Repositories.Categories.Base
{
    internal interface IDefaultWordsDatabase
    {
        List<WordEntry> WordEntries { get; }
    }
}