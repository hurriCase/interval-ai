using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.ApiHelper;
using Source.Scripts.Core.Localization.LanguageDetector;
using Source.Scripts.Core.Localization.Translator.Data;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.Networking;

namespace Source.Scripts.Core.Localization.Translator
{
    internal sealed class AzureTranslatorService : ApiServiceBase<AzureTranslationConfig>, ITranslator
    {
        private const string SubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
        private const string SubscriptionRegionHeader = "Ocp-Apim-Subscription-Region";

        private readonly ILanguageSettingsRepository _languageSettingsRepository;
        private readonly AzureTranslationConfig _azureTranslationConfig;
        private readonly ILanguageDetector _languageDetector;

        internal AzureTranslatorService(
            ILanguageSettingsRepository languageSettingsRepository,
            IApiAvailabilityChecker apiAvailabilityChecker,
            AzureTranslationConfig azureTranslationConfig,
            ILanguageDetector languageDetector,
            IApiClient apiClient)
            : base(apiAvailabilityChecker, apiClient, azureTranslationConfig)
        {
            _languageSettingsRepository = languageSettingsRepository;
            _azureTranslationConfig = azureTranslationConfig;
            _languageDetector = languageDetector;
        }

        public UniTask<string> TranslateTextAsync(string text, CancellationToken token)
        {
            var language = _languageDetector.DetectLanguage(text);
            var oppositeLanguage = _languageSettingsRepository.GetOppositeLanguage(language);
            return TranslateTextAsync(text, oppositeLanguage, token);
        }

        public async UniTask<string> TranslateTextAsync(
            string text,
            SystemLanguage targetLanguage,
            CancellationToken token)
        {
            var normalizedText = text?.Trim();
            if (string.IsNullOrEmpty(normalizedText))
                return string.Empty;

            _azureTranslationConfig.SetLanguageCode(targetLanguage);

            var requestBody = new[] { new TranslationRequest(normalizedText) };

            var response =
                await GetResponse<AzureTranslatorService, TranslationRequest[], TranslationResponse[]>(
                    this,
                    requestBody,
                    token,
                    static (self, request) => self.SetAzureHeaders(request));

            if (response is null || response.Length == 0)
                return normalizedText;

            var firstResult = response[0];
            if (firstResult?.Translations is null || firstResult.Translations.Length == 0)
                return normalizedText;

            return firstResult.Translations[0].Text;
        }

        private void SetAzureHeaders(UnityWebRequest request)
        {
            request.SetRequestHeader(SubscriptionKeyHeader, _azureTranslationConfig.ApiKey);
            request.SetRequestHeader(SubscriptionRegionHeader, _azureTranslationConfig.Region);
        }
    }
}