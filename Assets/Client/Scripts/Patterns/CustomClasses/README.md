# Unity Singleton Package

A collection of base classes for implementing different types of singletons in Unity.

## Available Singleton Types

### 1. SingletonBehaviour<T>
Base class for regular MonoBehaviour singletons. Use when you need a single instance that exists in the current scene.

```csharp
public class GameManager : SingletonBehaviour<GameManager>
{
    // Access via GameManager.Instance
}
```

Key features:
- Destroys any duplicate instances automatically
- Cleans up reference when destroyed
- Integrates with DI container

### 2. PersistentSingletonBehavior<T>
Base class for MonoBehaviour singletons that persist between scenes. Can load from prefab or create dynamically.

```csharp
[Resource(name: "AudioManager")] // Optional - will load from prefab if attribute present
public class AudioManager : PersistentSingletonBehavior<AudioManager>
{
    // Access via AudioManager.Instance
}
```

Key features:
- Persists through scene loads (DontDestroyOnLoad)
- Can load from prefab in Resources folder
- Creates GameObject dynamically if no prefab found
- Integrates with DI container

### 3. SingletonScriptableObject<T>
Base class for ScriptableObject singletons. Good for configuration and persistent data.

```csharp
[Resource(resourcePath: "Config", name: "GameSettings")]
public class GameSettings : SingletonScriptableObject<GameSettings>
{
    // Access via GameSettings.Instance
}
```

Key features:
- Loads from Resources folder
- Creates asset file automatically in editor if missing
- Requires ResourceAttribute for path configuration
- Editor support for asset creation/saving

### 4. Singleton<T>
Base class for regular C# class singletons. Use for non-Unity service classes.

```csharp
public class DataService : Singleton<DataService>
{
    // Access via DataService.Instance
}
```

Key features:
- Lazy initialization
- No Unity dependencies
- Simple and lightweight

## ResourceAttribute Configuration

Used to configure where singleton assets are loaded from/saved to:

```csharp
[Resource(
    assetPath: "Assets/Config/GameSettings.asset",  // Editor asset path
    resourcePath: "Config",                         // Runtime resource path
    name: "GameSettings"                           // Asset name
)]
```

## Best Practices

1. Choose the right singleton type:
    - `SingletonBehaviour` for scene-specific managers
    - `PersistentSingletonBehavior` for cross-scene systems
    - `SingletonScriptableObject` for configuration/data
    - `Singleton` for pure C# services

2. Override cleanup if needed:
```csharp
protected override void OnDestroy()
{
    // Your cleanup code
    base.OnDestroy();  // Always call base
}
```

3. Always specify ResourceAttribute path for ScriptableObject singletons:
```csharp
[Resource(resourcePath: "Config", name: "Settings")]
public class Settings : SingletonScriptableObject<Settings>
```

4. Consider using prefabs with PersistentSingletonBehavior for configurable components:
```csharp
[Resource(name: "AudioManager")]
public class AudioManager : PersistentSingletonBehavior<AudioManager>
{
    [SerializeField] private AudioSource _musicSource;
}
```

## Common Issues

- **Missing ResourceAttribute**: ScriptableObject singletons require the attribute
- **Duplicate Instances**: Check if something is instantiating the prefab directly
- **Null Instance**: Ensure singleton is initialized before use
- **Missing Prefab**: Verify prefab exists in Resources folder at specified path