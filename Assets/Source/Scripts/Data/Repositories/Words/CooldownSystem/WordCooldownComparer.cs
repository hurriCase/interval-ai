using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Words.Data;

namespace Source.Scripts.Data.Repositories.Words.CooldownSystem
{
    internal sealed class WordCooldownComparer : IComparer<WordEntry>
    {
        public int Compare(WordEntry x, WordEntry y)
        {
            if (x == null || y == null)
                return Comparer<WordEntry>.Default.Compare(x, y);

            return x.Cooldown != y.Cooldown
                ? x.Cooldown.CompareTo(y.Cooldown)
                : x.GUID.CompareTo(y.GUID);
        }
    }
}