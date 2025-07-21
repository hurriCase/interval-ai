using System.Collections.Generic;
using System.Runtime.Serialization;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Vocabulary
{
    internal sealed class WordCooldownComparer : IComparer<WordEntry>
    {
        private readonly ObjectIDGenerator _idGenerator = new();

        public int Compare(WordEntry x, WordEntry y)
        {
            if (x == null || y == null)
                return Comparer<WordEntry>.Default.Compare(x, y);

            if (x.Cooldown != y.Cooldown)
                return x.Cooldown.CompareTo(y.Cooldown);

            var xId = _idGenerator.GetId(x, out _);
            var yId = _idGenerator.GetId(y, out _);
            return xId.CompareTo(yId);
        }
    }
}