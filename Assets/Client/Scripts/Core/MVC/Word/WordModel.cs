using System.Collections.Generic;
using System.Linq;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.CategoryEntity;
using Client.Scripts.DB.Entities.EntityController;
using Client.Scripts.DB.Entities.WordEntity;
using Client.Scripts.MVC.Base;
using Client.Scripts.Patterns.DI.Base;

namespace Client.Scripts.Core.MVC.Word
{
    internal class WordModel : ModelBase<WordEntryContent>
    {
        [Inject] private IEntityController _entityController;

        internal string CategoryTitle
        {
            get
            {
                var categoryTitle = _entityController.FindEntries<CategoryEntity, CategoryEntryContent>(
                        category => category.Id == Data.Content.CategoryId).FirstOrDefault()
                    ?.Content.Title;
                return categoryTitle;
            }
        }

        internal string NativeWord => Data.Content.NativeWord;
        internal string LearningWord => Data.Content.LearningWord;
        internal string Transcription => Data.Content.Transcription;
        internal List<WordEntryContent.Example> Examples => Data.Content.Examples;

        internal WordModel(EntryData<WordEntryContent> data) : base(data) { }
    }
}