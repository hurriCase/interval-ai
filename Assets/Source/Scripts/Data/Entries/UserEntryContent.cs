using MemoryPack;

namespace Source.Scripts.Data.Entries
{
    [MemoryPackable]
    internal partial struct UserEntryContent
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}