using System.Globalization;
using MemoryPack;

namespace Source.Scripts.Data.Repositories.User
{
    [MemoryPackable]
    internal partial struct UserEntry
    {
        public string Name { get; }
        public CultureInfo CurrentCulture { get; }

        public UserEntry(string name, CultureInfo currentCulture)
        {
            Name = name;
            CurrentCulture = currentCulture;
        }
    }
}