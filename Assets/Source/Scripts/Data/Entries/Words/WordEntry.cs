using MemoryPack;

namespace Source.Scripts.Data.Entries.Words
{
    [MemoryPackable]
    internal partial struct WordEntry
    {
        internal LearningState LearningState { get; set; }
        internal Word NativeWord { get; set; }
        internal Word LearningWord { get; set; }
        internal string Transcription { get; set; }
    }
}