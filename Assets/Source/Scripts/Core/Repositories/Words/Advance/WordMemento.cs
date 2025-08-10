using MemoryPack;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Core.Repositories.Words.Advance
{
    internal struct WordMemento
    {
        private readonly byte[] _data;
        private WordEntry _wordEntry;

        public WordMemento(WordEntry wordEntry)
        {
            _data = MemoryPackSerializer.Serialize(wordEntry);
            _wordEntry = wordEntry;
        }

        public void Undo()
        {
            MemoryPackSerializer.Deserialize(_data, ref _wordEntry);
        }
    }
}