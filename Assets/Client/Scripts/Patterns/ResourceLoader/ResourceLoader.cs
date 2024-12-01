using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Client.Scripts.ResourceLoader
{
    internal static class ResourceLoader
    {
        private const string DontDestroyPath = "DontDestroyOnLoad/";
        private static readonly Dictionary<string, Object> _resourceCache = new();
        private static readonly Dictionary<string, Array> _resourceArrayCache = new();

        internal static TResourceType Load<TResourceType>(string path) where TResourceType : Object
            => LoadWithCache<TResourceType>(path);

        internal static bool TryLoad<TResourceType>(string path, out TResourceType resource)
            where TResourceType : Object
        {
            try
            {
                resource = LoadWithCache<TResourceType>(path);
                return resource != null;
            }
            catch (ResourceLoadException)
            {
                resource = null;
                return false;
            }
        }

        internal static TResourceType LoadDontDestroy<TResourceType>() where TResourceType : Object
            => LoadWithCache<TResourceType>(DontDestroyPath);

        internal static bool TryLoadDontDestroy<TResourceType>(out TResourceType resource)
            where TResourceType : Object
        {
            try
            {
                resource = LoadWithCache<TResourceType>(DontDestroyPath);
                return resource != null;
            }
            catch (ResourceLoadException)
            {
                resource = null;
                return false;
            }
        }

        internal static TResourceType[] LoadAll<TResourceType>(string path) where TResourceType : Object
            => LoadAllWithCache<TResourceType>(path);

        internal static TResourceType[] LoadAllDontDestroy<TResourceType>() where TResourceType : Object
            => LoadAllWithCache<TResourceType>(DontDestroyPath);

        internal static void ClearCache()
        {
            _resourceCache.Clear();
            _resourceArrayCache.Clear();
        }

        internal static void RemoveFromCache(string path)
        {
            _resourceCache.Remove(path);
            _resourceArrayCache.Remove(path);
        }

        private static TResourceType LoadWithCache<TResourceType>(string path) where TResourceType : Object
        {
            var validatedPath = ValidatePath(path);

            var cacheKey = $"{typeof(TResourceType).Name}:{validatedPath}";

            if (_resourceCache.TryGetValue(cacheKey, out var cachedResource))
                return cachedResource as TResourceType;

            var resource = Resources.Load<TResourceType>(path);

            if (resource == null)
            {
                var error =
                    $"[ResourceLoader::Load] Failed to load resource at path: {validatedPath} of type: {typeof(TResourceType).Name}";
                Debug.LogError(error);
                throw new ResourceLoadException(error);
            }

            _resourceCache[cacheKey] = resource;
            return resource;
        }

        private static TResourceType[] LoadAllWithCache<TResourceType>(string path) where TResourceType : Object
        {
            var validatedPath = ValidatePath(path);

            var cacheKey = $"{typeof(TResourceType).Name}[]:{validatedPath}";

            if (_resourceArrayCache.TryGetValue(cacheKey, out var cachedResources))
                return (TResourceType[])cachedResources;

            var resources = Resources.LoadAll<TResourceType>(validatedPath);

            if (resources == null || resources.Length == 0)
            {
                var error =
                    $"[ResourceLoader::LoadAll] No resources found at path: {validatedPath} of type: {typeof(TResourceType).Name}";
                Debug.LogError(error);
                throw new ResourceLoadException(error);
            }

            _resourceArrayCache[cacheKey] = resources;
            return resources;
        }

        private static string ValidatePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ResourceLoadException("[ResourceLoader] Path cannot be null or empty");

            if (path.Contains(".."))
                throw new ResourceLoadException("[ResourceLoader] Path cannot contain parent directory references");

            return path.TrimStart('/');
        }
    }

    internal class ResourceLoadException : Exception
    {
        public ResourceLoadException(string message) : base(message) { }
    }
}