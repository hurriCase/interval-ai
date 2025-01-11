using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.EntityController;
using Client.Scripts.DB.Entities.WordEntity;
using Client.Scripts.MVC.Base;

namespace Client.Scripts.Core.MVC.Word
{
    internal class WordController : ControllerBase<WordEntity, WordEntryContent, WordModel>
    {
        internal WordController(IEntityController entityController, IView<WordModel> view)
            : base(entityController, view) { }

        internal async Task<bool> CreateWord(string categoryId, string nativeWord, string learningWord,
            string transcription, List<WordEntryContent.Example> examples)
        {
            var content = new WordEntryContent
            {
                CategoryId = categoryId,
                NativeWord = nativeWord,
                LearningWord = learningWord,
                Transcription = transcription,
                Examples = examples
            };

            return await CreateEntry(content);
        }

        protected override WordModel CreateModel(EntryData<WordEntryContent> data) => new(data);
    }
}