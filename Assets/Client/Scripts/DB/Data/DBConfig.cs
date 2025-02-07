using AssetLoader.Runtime;
using CustomClasses.Runtime.Singletons;
using UnityEngine;

namespace Client.Scripts.DB.Data
{
    [Resource("Assets/Resources/Configs", "DBConfig", "Configs")]
    internal sealed class DBConfig : SingletonScriptableObject<DBConfig>
    {
        [field: SerializeField] internal string UserPath { get; private set; } = "users";
        [field: SerializeField] internal string UserDataPath { get; private set; } = "user_data";
        [field: SerializeField] internal string ConfigsPath { get; private set; } = "configs";
        [field: SerializeField] internal string AIConfigPath { get; private set; } = "ai_config";
        [field: SerializeField] internal string ValidationRulesPath { get; private set; } = "validation_config";
        [field: SerializeField] internal string AIChatHistoryPath { get; private set; } = "ai_chat_history";
        [field: SerializeField] internal string TestsPath { get; private set; } = "tests";
        [field: SerializeField] internal string BackupPrefix { get; private set; } = "backup";
    }
}