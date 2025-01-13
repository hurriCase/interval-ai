using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Core.MVC.Base;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.EntityController;
using Client.Scripts.DB.Entities.UserCategory;
using Client.Scripts.DB.Entities.Word;
using Client.Scripts.Patterns.Attributes;

namespace Client.Scripts.Core.MVC.Word
{
    internal sealed class WordModel : ModelBase<WordEntryContent>
    {
        [Inject] private IEntityController _entityController;

        internal string CategoryTitle
        {
            get
            {
                var categoryTitle = _entityController.FindEntries<UserCategoryEntity, UserCategoryEntryContent>(
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