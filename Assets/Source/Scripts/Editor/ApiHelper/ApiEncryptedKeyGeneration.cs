using CustomUtils.Editor.Scripts.CustomEditorUtilities;
using CustomUtils.Runtime.Encryption;
using CustomUtils.Runtime.Extensions;
using UnityEditor;

namespace Source.Scripts.Editor.ApiHelper
{
    internal sealed class ApiEncryptedKeyGeneration : WindowBase
    {
        private string _apiKey;
        private string _generatedEncryptedKey;

        [MenuItem("Tools/Generate Api Encrypted Key")]
        internal static void ShowWindow()
        {
            GetWindow<ApiEncryptedKeyGeneration>(nameof(ApiEncryptedKeyGeneration).ToSpacedWords());
        }

        protected override void DrawWindowContent()
        {
            _apiKey = EditorStateControls.TextField("ApiKey", _apiKey);
            _generatedEncryptedKey =
                EditorStateControls.TextField("Generate Api Encrypted Key", _generatedEncryptedKey);

            if (EditorVisualControls.Button("Generate Api Encrypted Key"))
                GenerateEncryptedKey();
        }

        private void GenerateEncryptedKey()
        {
            if (BuildConstants.PasswordEnvironmentVariable.TryGetValueFromEnvironment(out var password) is false)
                return;

            _generatedEncryptedKey = XORDataEncryption.Encrypt(_apiKey, password);
        }
    }
}