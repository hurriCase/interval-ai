using MemoryPack;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Localization.Translator.Translations
{
    [MemoryPackable]
    internal readonly partial struct Translation : ITranslation
    {
        public string Learning { get; }
        public string Native { get; }

        public Translation(string learning, string native)
        {
            Learning = learning;
            Native = native;
        }

        [MemoryPackIgnore]
        public bool IsValid => string.IsNullOrEmpty(Native) is false && string.IsNullOrEmpty(Learning) is false;
    }
}