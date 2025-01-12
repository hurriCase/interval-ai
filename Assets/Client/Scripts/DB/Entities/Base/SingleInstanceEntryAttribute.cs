using System;

namespace Client.Scripts.DB.Entities.Base
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class SingleInstanceEntryAttribute : Attribute { }
}