using Client.Scripts.Core;
using UnityEngine;

namespace Client.Scripts.DB.DBControllers
{
    internal sealed class DBConfig : ScriptableObject
    {
        internal static DBConfig Instance => _instance ?? (_instance = ConfigLoader.LoadDBConfig<DBConfig>());
        private static DBConfig _instance;

        [field: SerializeField] internal string UserPath { get; private set; } = "users";
        [field: SerializeField] internal string UserDataPath { get; private set; } = "user_data";
        [field: SerializeField] internal string ConfigsPath { get; private set; } = "configs";
        [field: SerializeField] internal string AIConfigPath { get; private set; } = "ai_config";
        [field: SerializeField] internal string AIChatHistoryPath { get; private set; } = "ai_chat_history";
        [field: SerializeField] internal string TestsPath { get; private set; } = "tests";
        [field: SerializeField] internal string BackupPrefix { get; private set; } = "backup";
    }
}