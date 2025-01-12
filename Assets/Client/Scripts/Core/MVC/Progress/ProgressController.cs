using System;
using System.Threading.Tasks;
using Client.Scripts.Core.MVC.Base;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.EntityController;
using Client.Scripts.DB.Entities.ProgressEntity;

namespace Client.Scripts.Core.MVC.Progress
{
    internal class ProgressController : ControllerBase<ProgressEntity, ProgressEntryContent, ProgressModel>
    {
        internal ProgressController(IEntityController entityController, IView<ProgressModel> view)
            : base(entityController, view) { }

        internal async Task<bool> CreateProgress(string wordId)
        {
            var content = new ProgressEntryContent
            {
                WordId = wordId,
                RepetitionStage = 0,
                TotalReviews = 0,
                CorrectReviews = 0,
                LastReviewDate = DateTime.Now,
                NextReviewDate = DateTime.Now
            };

            return await CreateEntry(content);
        }

        protected override ProgressModel CreateModel(EntryData<ProgressEntryContent> data) => new(data);
    }
}