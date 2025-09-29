# LoopLang AI Wiki

## Table of Contents
- [Project Overview](#project-overview)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [App Flow](#app-flow)
- [Systems](#systems)
- [Coding Standards](#coding-standards)
- [Asset Guidelines](#asset-guidelines)

## Project Overview

### App Description
2D Language Learning Application with AI features designed to help people learn new languages easily. Core mechanics include:
- **Flash Cards System:** Study new words/grammar with spaced repetition
- **AI-Generated Exercises:** Sentence-based exercises with learning words for contextual memorization

### Technical Specifications
- **Unity Version:** 6000.2.0f1
- **Target Platforms:** Mobile (Android primary), Web (future)
- **Core Architecture:** Dependency Injection (VContainer) + Reactive Programming (R3)

### Team & Roles
| Name    | Role      | Responsibilities               |
|---------|-----------|--------------------------------|
| Dmitrii | Developer | Technical implementation and development oversight |
| Lidia   | Designer  | Design direction and visual implementation |

## Getting Started

### Repository Setup
1. Clone the repository: `git clone https://github.com/hurriCase/interval-ai.git`
2. Open project in Unity Hub
3. Set target platform to Android (recommended)

## Project Structure
```
Assets/Source/
├── Fonts/               # Typography assets
├── Prefabs/
├── Resources/           # Runtime-loaded assets (but prefer Addressables for dynamic loading instead)
├── Scenes/
├── Scriptables/         # ScriptableObject configs
├── Scripts/
│   ├── Bootstrap/        # DI setup and entry points
│   ├── Core/             # Core systems and repositories
│   ├── Editor/           # Editor tools and utilities
│   ├── Main/             # Main scene UI logic
│   ├── Onboarding/       # Onboarding scene UI logic
│   └── UI/               # Core UI systems
├── Sound/
├── Sprites/             # 2D textures and sprites
└── Texts/               # Text assets and localization
```

### Assembly Definitions
- **Bootstrap:** DI configuration and application bootstrap
- **Core:** Systems, repositories, and utilities
- **Editor:** Development tools and data creation utilities
- **Main:** Primary application screens and functionality
- **Onboarding:** User onboarding flow
- **UI:** Core UI components and window controllers

## App Flow
1. [ProjectWideLifetimeScope](Assets/Source/Scripts/Bootstrap/DI/ProjectWideLifetimeScope.cs "ProjectWideLifetimeScope class"): Core dependencies initialization
2. [EntryPoint](Assets/Source/Scripts/Bootstrap/Core/EntryPoint.cs "EntryPoint class"): Steps initialization
3. [OnboardingLifetimeScope](Assets/Source/Scripts/Onboarding/DI/OnboardingLifetimeScope.cs "OnboardingLifetimeScope class"): Onboarding dependencies (if not completed)
4. [OnboardingEntryPoint](Assets/Source/Scripts/Onboarding/DI/OnboardingEntryPoint.cs "OnboardingEntryPoint class"): Onboarding setup (if not completed)
5. [MainLifetimeScope](Assets/Source/Scripts/Main/DI/MainLifetimeScope.cs "MainLifetimeScope class"): Main scene dependencies
6. [MainEntryPoint](Assets/Source/Scripts/Main/DI/MainEntryPoint.cs "MainEntryPoint class"): Main scene setup
7. [WindowsController](Assets/Source/Scripts/UI/Windows/Base/WindowsController.cs "WindowsController class"): UI initialization

## Systems

### Dependency Injection ([VContainer](https://vcontainer.hadashikick.jp/))

**Purpose**: Service lifetime management and dependency resolution
- Global services managed by [ProjectWideLifetimeScope](Assets/Source/Scripts/Bootstrap/DI/ProjectWideLifetimeScope.cs "ProjectWideLifetimeScope class")
- Scene-specific services in dedicated lifetime scopes:
    - [OnboardingLifetimeScope](Assets/Source/Scripts/Onboarding/DI/OnboardingLifetimeScope.cs "OnboardingLifetimeScope class")
    - [MainLifetimeScope](Assets/Source/Scripts/Main/DI/MainLifetimeScope.cs "MainLifetimeScope class")

### Repository Pattern

**Purpose**: Centralized data management with persistent reactive properties and automatic persistence
- [CategoriesRepository](Assets/Source/Scripts/Core/Repositories/Categories/CategoriesRepository.cs "CategoriesRepository class") - Category management
- [WordsRepository](Assets/Source/Scripts/Core/Repositories/Words/WordsRepository.cs "WordsRepository class") - Word entries and progress
- Settings repositories for different domains:
    - [LanguageSettingsRepository](Assets/Source/Scripts/Core/Repositories/Settings/Repositories/LanguageSettingsRepository.cs "LanguageSettingsRepository class")
    - [PracticeSettingsRepository](Assets/Source/Scripts/Core/Repositories/Settings/Repositories/PracticeSettingsRepository.cs "PracticeSettingsRepository class")
    - [GenerationSettingsRepository](Assets/Source/Scripts/Core/Repositories/Settings/Repositories/GenerationSettingsRepository.cs "GenerationSettingsRepository class")
    - [UISettingsRepository](Assets/Source/Scripts/Core/Repositories/Settings/Repositories/UISettingsRepository.cs "UISettingsRepository class")
- [ProgressRepository](Assets/Source/Scripts/Core/Repositories/Progress/ProgressRepository.cs "ProgressRepository class") - Learning progress tracking
- [StatisticsRepository](Assets/Source/Scripts/Core/Repositories/Statistics/StatisticsRepository.cs "StatisticsRepository class") - User statistics
- [UserRepository](Assets/Source/Scripts/Core/Repositories/User/UserRepository.cs "UserRepository class") - User profile data

### WindowsController ([Custom](Assets/Source/Scripts/UI/Windows/Base/WindowsController.cs "WindowsController class"))

**Purpose**: UI navigation and lifecycle management
- **Screens:** Full-screen UI for primary navigation, see [LearningWordScreen](Assets/Source/Scripts/Main/UI/Screens/LearningWords/LearningWordScreen.cs "LearningWordScreen class")
- **PopUps:** Overlay UI with stacking and back navigation, see [WordPracticePopUp](Assets/Source/Scripts/Main/UI/PopUps/WordPractice/WordPracticePopUp.cs "WordPracticePopUp class")
- Two popup opening approaches: direct type-based (`OpenPopUpByType`) or generic with parameters (`OpenPopUp<T>`)
- See [MenuBehaviour](Assets/Source/Scripts/Main/UI/Base/MenuBehaviour.cs "MenuBehaviour class") for screen navigation example

### Localization System ([Custom](https://github.com/hurriCase/CustomUtils/tree/main/Runtime/Localization))

**Purpose**: Provides localization for the entire application based on Google Sheets
- Static text: Use [LocalizedTextMeshPro](https://github.com/hurriCase/CustomUtils/blob/main/Runtime/Localization/LocalizedTextMeshPro.cs "LocalizedTextMeshPro class") component on UI elements
- Dynamic text: Use [LocalizationController](https://github.com/hurriCase/CustomUtils/blob/main/Runtime/Localization/LocalizationController.cs "LocalizationController class") for runtime localization. See [ProgressDescriptionItem](Assets/Source/Scripts/Main/UI/PopUps/Achievement/Behaviours/LearningStarts/ProgressDescriptionItem.cs "ProgressDescriptionItem class") for usage example

### Custom EnumArray collection ([Custom](https://github.com/hurriCase/CustomUtils/tree/main/Runtime/CustomTypes/Collections))

**Purpose**: Collection that uses enum values as keys with struct-based enumeration
- Supports both enum key and integer index access
- See [MenuBehaviour](Assets/Source/Scripts/Main/UI/Base/MenuBehaviour.cs "MenuBehaviour class") for usage example

### Audio System ([Custom](https://github.com/hurriCase/CustomUtils/tree/main/Runtime/Audio))

**Purpose**: Sound effects and audio feedback
- See [ButtonComponent](Assets/Source/Scripts/UI/Components/Button/ButtonComponent.cs "ButtonComponent class") for usage example

### CSV Data Processing ([Custom](https://github.com/hurriCase/CustomUtils/tree/main/Runtime/CSV))

**Purpose**: Provides the ability to set data for application, for default data and user defined as well
- Per-type converter approach instead of reflection-based general solution
- Implement CsvConverterBase inheritance to define type conversion logic.
  See [WordConverter](Assets/Source/Scripts/Core/Repositories/Words/Word/WordEntry.WordConverter.cs "WordConverter class") for usage example
- Pre-serialized data shipping to avoid runtime CSV conversion overhead through [DefaultDataWindow](Assets/Source/Scripts/Editor/DefaultDataCreation/DefaultEntriesCreatorWindow.cs "DefaultDataWindow class")

### Serialization ([MemoryPack](https://github.com/Cysharp/MemoryPack))

**Purpose**: Handles binary serialization for persistent data storage through `PersistentReactiveProperty<T>`
and complex data structures.
- Important version-tolerant restrictions: [MemoryPack Version Tolerance](https://github.com/Cysharp/MemoryPack?tab=readme-ov-file#version-tolerant)
- Serializable Types Must:
    - Be marked with `[MemoryPackable]` attribute
    - Use `partial` keyword for classes/structs
    - Members inside have `public` accessibility
    - Use `[MemoryPackIgnore]` for computed/non-serializable properties
    - See [Translation](Assets/Source/Scripts/Core/Repositories/Words/Word/Translation.cs "Translation class") for usage example


### API ([Custom](Assets/Source/Scripts/Core/Api/ApiClient.cs "ApiClient class"))

**Purpose**: Third-party service integration with type-safe JSON parsing
- Examples:
    - [GeminiGenerativeLanguageService](Assets/Source/Scripts/Core/GenerativeLanguage/GeminiGenerativeLanguageService.cs "GeminiGenerativeLanguageService class") - Generative Language
    - [GoogleTextToSpeechService](Assets/Source/Scripts/Core/Audio/TextToSpeech/GoogleTextToSpeechService.cs "GoogleTextToSpeechService class") - Text-to-speech
    - [AzureTranslatorService](Assets/Source/Scripts/Core/Localization/Translator/AzureTranslatorService.cs "AzureTranslatorService class") - Translation

## Coding Standards

### General Guidelines
- Use `var` keyword where type is obvious
- Use static comparison for boolean negation
  (except GameObject types where it doesn't check for destruction): `if (isActive is false)`
- Invert if statements to reduce nesting where reasonable
- Use auto-properties with field serialization: `[field: SerializeField] internal string Name { get; private set; }`
- Method names should be verbs describing the action performed: `CalculateScore()`, `ValidateInput()`, `UpdatePosition()`
- Property names should be nouns or adjectives describing state: `PlayerHealth`, `IsVisible`, `MaxCapacity`
- Field names should be nouns representing stored data: `_currentScore`, `_targetPosition`, `_audioSource`
- Observables should use `On` prefix for events: `OnDataChanged`, `OnValidationCompleted`
- Subjects should use clear action-based names: `_dataChanged`, `_validationCompleted`
- All async methods must have `Async` suffix
- Use ZString for string manipulations
- Use None/Default for first enum value; specify explicit integer values for all enum members
- Avoid magic numbers/strings; create named constants for all literal values
- Avoid public fields; use properties with appropriate access modifiers
- Use internal accessibility wherever possible instead of public
- Use expression body syntax for single-line return methods: `internal bool IsValid() => _data != null;`
- Prevent closure allocation in lambda expressions: `.Subscribe((data, self), static (value, tuple) => tuple.self.Method(value))`
- Use meaningful names for generic parameters when possible: `<TSelf>` instead of `<T>`


### Naming Conventions
- **Classes:** PascalCase (`CategoryRepository`)
- **Structs:** PascalCase (`CachedSprite`)
- **Enums:** PascalCase (`LearningState`)
- **Interfaces:** PascalCase with 'I' prefix (`ICategoriesRepository`)
- **Methods:** PascalCase (`InitAsync()`)
- **Variables:** camelCase (`currentWords`)
- **Constants:** PascalCase (`MaxWeeksInMonth`)
- **Public|Internal Properties:** PascalCase (`CategoriesRepository`)
- **Private fields:** camelCase with underscore prefix (`_categoriesRepository`)
- **Protected fields:** camelCase (`categoriesRepository`)

### Member Order in Types
1. **Unity Serialized Properties** (`[field: SerializeField]` auto-properties)
2. **Unity Serialized Fields** (`[SerializeField]` fields)
3. **Properties** (non-Unity serialized)
4. **Constants** (`const`, `static readonly`)
5. **Fields** (non-Unity serialized)
6. **Constructor related members**
7. **Constructors**
8. **Methods**

**Accessibility order within each member type:**
- `public` → `internal` → `protected` → `private`

**Example:**
```csharp
internal sealed class WordRepository : IWordsRepository
{
    [field: SerializeField] internal WordsConfig Config { get; private set; }
    
    [SerializeField] private WordEntry _wordEntryPrefab;
    
    public ReactiveProperty<WordEntry[]> Words { get; }

    internal bool IsInitialized => _words.Count > 0;
    
    private const int MaxWordsCount = 1000;
    
    private readonly List<WordEntry> _words = new();
    
    private readonly IWordsService _wordsService;
    
    public WordRepository(IWordsService wordsService) 
    {
        _wordsService = wordsService;
    }
    
    public async UniTask InitAsync() => await LoadWordsAsync();
    private async UniTask LoadWordsAsync() { /* implementation */ }
}
```

### Code Organization
- One class per file, separate artifacts per class
- Assembly definitions organize code by domain
- Async/await pattern with UniTask for asynchronous operations
- Encapsulation priority: get-only properties, nested classes for state mutation
- Use scriptable objects for static data configs/database, or as provides some files

### Unity-Specific Guidelines
- Prefer `[field: SerializeField]` auto-properties over private fields with public getters
- Use VContainer for dependency injection instead of singletons
- Cache component references through dependency injection
- Use Addressables for dynamic asset loading
- Avoid GetComponent calls; assign references through serialized properties

## Asset Guidelines

### File Naming Conventions
- **FA_** - Fonts (`FA_MainFont`)
- **T_** - Textures (`T_ButtonBackground`)
- **P_** - Prefabs (`P_WordCard`)
- **SC_** - Sound Clips (`SC_ButtonClick`)
- **M_** - Materials (`M_ProceduralImage`)
- **SD_** - Shaders (`SD_Gradient`)

### Organization Principles
- Group assets by feature/system within assembly boundaries
- Use Addressables for runtime asset loading
- Encapsulate everything: create get-only properties, nested classes for state mutation
- Ship project on prefab basis without storing assets directly in scenes
- Maintain consistent folder structure across features

*Last Updated: 2025-09-11*