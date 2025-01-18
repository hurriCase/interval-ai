using System;

namespace Client.Scripts.Patterns.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class ResourceAttribute : Attribute
    {
        internal string Name { get; }
        internal string Path { get; }

        private readonly string _defaultPath = "DontDestroyOnLoad/";

        internal ResourceAttribute(string name)
        {
            Name = name;
            Path = _defaultPath + name;
        }
    }
}