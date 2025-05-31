using MemoryPack;

// ReSharper disable MemberCanBePrivate.Global
namespace Client.Scripts.Data.Client.Scripts.Data.Entries
{
    [MemoryPackable]
    internal partial struct Example
    {
        public string NativeSentence { get; set; }
        public string LearningSentence { get; set; }
    }
}