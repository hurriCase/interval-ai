using MemoryPack;

namespace Source.Scripts.Core.Repositories.Categories
{
    [MemoryPackable]
    internal sealed partial class DefaultCategoryEntry : IDefaultEntry<CategoryEntry>
    {
        public int DefaultId { get; set; }
        public CategoryEntry Entry { get; set; }
    }
}