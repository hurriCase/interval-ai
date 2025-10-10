using System;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    internal readonly struct SaveScope : IDisposable
    {
        private readonly WordsRepository _wordsRepository;
        private readonly Action<WordsRepository> _saveCallback;

        internal SaveScope(WordsRepository wordsRepository, Action<WordsRepository> saveCallback)
        {
            _wordsRepository = wordsRepository;
            _saveCallback = saveCallback;
        }

        public void Dispose()
        {
            _saveCallback.Invoke(_wordsRepository);
        }
    }
}