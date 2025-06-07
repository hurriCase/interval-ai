using MemoryPack;

// ReSharper disable MemberCanBePrivate.Global
namespace Source.Scripts.Data.Entries
{
    [MemoryPackable]
    internal partial struct Example
    {
        public string NativeSentence { get; set; }
        public string LearningSentence { get; set; }
    }
}