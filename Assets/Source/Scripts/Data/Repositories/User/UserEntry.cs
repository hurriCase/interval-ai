using MemoryPack;

namespace Source.Scripts.Data.Repositories.User
{
    [MemoryPackable]
    internal partial struct UserEntry
    {
        public string Name { get; set; }

        public UserEntry(string name)
        {
            Name = name;
        }
    }
}