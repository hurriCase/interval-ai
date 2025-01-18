using System;

namespace Client.Scripts.Patterns.Singletons
{
    internal abstract class Singleton<T> where T : class
    {
        private static T _instance;

        internal static T Instance
        {
            get { return _instance = _instance ?? (_instance = Activator.CreateInstance(typeof(T)) as T); }
        }
    }
}