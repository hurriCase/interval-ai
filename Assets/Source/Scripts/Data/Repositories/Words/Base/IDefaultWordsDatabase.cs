using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Words.Data;

namespace Source.Scripts.Data.Repositories.Words.Base
{
    internal interface IDefaultWordsDatabase
    {
        List<WordEntry> WordEntries { get; }
    }
}