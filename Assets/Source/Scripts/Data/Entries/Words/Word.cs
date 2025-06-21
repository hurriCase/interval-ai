using MemoryPack;

namespace Source.Scripts.Data.Entries.Words
{
    [MemoryPackable]
    internal partial struct Word
    {
        internal string WordName { get; set; }
        internal Language Language { get; set; }
    }
}