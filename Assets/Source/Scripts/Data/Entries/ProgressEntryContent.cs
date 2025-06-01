using System;
using MemoryPack;

// ReSharper disable MemberCanBePrivate.Global
namespace Client.Scripts.Data.Client.Scripts.Data.Entries
{
    [MemoryPackable]
    internal partial struct ProgressEntryContent
    {
        public string WordId { get; set; }
        public int RepetitionStage { get; set; }
        public int TotalReviews { get; set; }
        public int CorrectReviews { get; set; }
        public DateTime LastReviewDate { get; set; }
        public DateTime NextReviewDate { get; set; }
    }
}