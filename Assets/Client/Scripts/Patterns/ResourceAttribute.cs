using System;

namespace Client.Scripts.Patterns
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class ResourceAttribute : Attribute
    {
        internal string Name { get; }
        internal string Path { get; }

        internal ResourceAttribute(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}