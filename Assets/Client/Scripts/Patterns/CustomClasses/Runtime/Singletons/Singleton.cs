using System;

namespace CustomClasses.Runtime.Singletons
{
    /// <summary>
    /// Base class for implementing the Singleton pattern for classes.
    /// Creates instance lazily on first access.
    /// </summary>
    /// <typeparam name="T">The type to make a singleton.</typeparam>
    public abstract class Singleton<T> where T : class
    {
        private static T _instance;

        public static T Instance
        {
            get { return _instance = _instance ?? (_instance = Activator.CreateInstance(typeof(T)) as T); }
        }
    }
}