using MemoryPack;
using Source.Scripts.Core.DI.Repositories.Words;

namespace Source.Scripts.Data.Repositories.Words.Advance
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