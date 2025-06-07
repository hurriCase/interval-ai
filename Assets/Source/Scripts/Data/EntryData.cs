using System;
using MemoryPack;

// ReSharper disable MemberCanBePrivate.Global
namespace Source.Scripts.Data
{
    [MemoryPackable]
    internal partial struct EntryData<TContent> where TContent : struct
    {
        public string Id { get; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }
        public TContent Content { get; }

        public EntryData(TContent content)
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Content = content;
        }

        [MemoryPackConstructor]
        public EntryData(string id, DateTime createdAt, DateTime updatedAt, TContent content)
        {
            Id = id;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Content = content;
        }
    }
}