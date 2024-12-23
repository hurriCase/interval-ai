using System;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.ProgressEntity;
using Client.Scripts.MVC.Base;

namespace Client.Scripts.Core.MVC.Progress
{
    internal class ProgressModel : ModelBase<ProgressEntryContent>
    {
        internal int RepetitionStage => Data.Content.RepetitionStage;
        internal int TotalReviews => Data.Content.TotalReviews;
        internal int CorrectReviews => Data.Content.CorrectReviews;
        internal DateTime LastReviewDate => Data.Content.LastReviewDate;
        internal DateTime NextReviewDate => Data.Content.NextReviewDate;

        internal ProgressModel(EntryData<ProgressEntryContent> data) : base(data) { }
    }
}