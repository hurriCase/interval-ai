using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.Downloader;

namespace Source.Scripts.Editor.DefaultDataCreation
{
    [Resource(
        LocalizationSettingsFullPath,
        LocalizationSettingsAssetName
    )]
    internal sealed class DefaultDataDatabase : SheetsDatabase<DefaultDataDatabase, DefaultDataSheet>
    {
        private const string LocalizationSettingsFullPath = "Assets/Resources";
        private const string LocalizationSettingsAssetName = "DefaultDataDatabase";

        public const string BinaryPath = TextsPath + "/Binary";
        private const string TextsPath = "Assets/Source/Texts";

        public override string GetDownloadPath() => TextsPath + "/CSV";
    }
}