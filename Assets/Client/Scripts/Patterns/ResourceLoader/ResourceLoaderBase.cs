using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Client.Scripts.Patterns.ResourceLoader
{
    internal abstract class ResourceLoaderBase<TResource> where TResource : Object
    {
        private readonly Dictionary<string, TResource> _resourceCache = new();
        private readonly Dictionary<string, TResource[]> _resourceArrayCache = new();

        protected abstract string BasePath { get; }

        protected TResource Load(string path)
        {
            var fullPath = GetFullPath(path);
            var cacheKey = GetCacheKey(fullPath);

            if (_resourceCache.TryGetValue(cacheKey, out var cached))
                return cached;

            var resource = Resources.Load<TResource>(fullPath);
            if (resource is null)
            {
                var error = $"[{GetType().Name}] Failed to load resource at path: {fullPath}";
                Debug.LogError(error);
                throw new ResourceLoadException(error);
            }

            _resourceCache[cacheKey] = resource;
            return resource;
        }

        protected bool TryLoad(string path, out TResource resource)
        {
            try
            {
                resource = Load(path);
                return true;
            }
            catch (ResourceLoadException)
            {
                resource = null;
                return false;
            }
        }

        protected TResource[] LoadAll(string subfolder = null)
        {
            var fullPath = GetFullPath(subfolder);
            var cacheKey = GetCacheKey(fullPath);

            if (_resourceArrayCache.TryGetValue(cacheKey, out var cached))
                return cached;

            var resources = Resources.LoadAll<TResource>(fullPath);
            if (resources == null || resources.Length == 0)
            {
                var error = $"[{GetType().Name}] No resources found at path: {fullPath}";
                Debug.LogError(error);
                throw new ResourceLoadException(error);
            }

            _resourceArrayCache[cacheKey] = resources;
            return resources;
        }

        protected void ClearCache()
        {
            _resourceCache.Clear();
            _resourceArrayCache.Clear();
        }

        protected void RemoveFromCache(string path)
        {
            var fullPath = GetFullPath(path);
            var cacheKey = GetCacheKey(fullPath);
            
            _resourceCache.Remove(cacheKey);
            _resourceArrayCache.Remove(cacheKey);
        }

        private string GetFullPath(string path) => 
            string.IsNullOrEmpty(path) ? BasePath : $"{BasePath}{path}";

        private string GetCacheKey(string path) => 
            $"{typeof(TResource).Name}:{ValidatePath(path)}";

        private static string ValidatePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ResourceLoadException("[ResourceLoader] Path cannot be null or empty");

            if (path.Contains(".."))
                throw new ResourceLoadException("[ResourceLoader] Path cannot contain parent directory references");

            return path.TrimStart('/');
        }
    }

    internal sealed class ResourceLoadException : Exception
    {
        public ResourceLoadException(string message) : base(message) { }
    }
}