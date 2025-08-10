using System.Collections.Generic;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Core.Repositories.Words.CooldownSystem
{
    internal sealed class WordCooldownComparer : IComparer<WordEntry>
    {
        public int Compare(WordEntry firstWord, WordEntry secondWord)
        {
            switch (firstWord)
            {
                case null when secondWord is null:
                    return 0;
                case null:
                    return -1;
            }

            if (secondWord is null)
                return 1;

            if (ReferenceEquals(firstWord, secondWord))
                return 0;

            var cooldownComparison = firstWord.Cooldown.CompareTo(secondWord.Cooldown);
            return cooldownComparison != 0
                ? cooldownComparison
                : firstWord.GetHashCode().CompareTo(secondWord.GetHashCode());
        }
    }
}