using System;

namespace Client.Scripts.DB.Entities.Base
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class SingleInstanceEntryAttribute : Attribute { }
}