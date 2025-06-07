using System.Collections.Generic;
using MemoryPack;

namespace Source.Scripts.Data.Entries
{
    [MemoryPackable]
    internal partial struct WordEntryContent
    {
        public string CategoryId { get; set; }
        public string NativeWord { get; set; }
        public string LearningWord { get; set; }
        public string Transcription { get; set; }
        public List<Example> Examples { get; set; }
        public bool IsDefault { get; set; }
    }
}