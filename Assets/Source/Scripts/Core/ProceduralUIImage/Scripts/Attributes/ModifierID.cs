using System;

namespace UnityEngine.UI.ProceduralImage
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModifierID : Attribute
    {
        public string Name { get; }

        public ModifierID(string name)
        {
            Name = name;
        }
    }
}