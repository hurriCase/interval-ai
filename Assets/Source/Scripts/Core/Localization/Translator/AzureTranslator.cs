using System.Threading;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.ApiHelper;
using Source.Scripts.Core.Localization.LanguageDetector;
using Source.Scripts.Core.Localization.Translator.Data;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.Networking;

namespace Source.Scripts.Core.Localization.Translator
{
    internal sealed class AzureTranslator : ITranslator
    {
        private readonly ILanguageSettingsRepository _languageSettingsRepository;
        private readonly AzureTranslationConfig _azureTranslationConfig;
        private readonly ILanguageDetector _languageDetector;
        private readonly IApiHelper _apiHelper;

        internal AzureTranslator(
            ILanguageSettingsRepository languageSettingsRepository,
            AzureTranslationConfig azureTranslationConfig,
            ILanguageDetector languageDetector,
            IApiHelper apiHelper)
        {
            _languageSettingsRepository = languageSettingsRepository;
            _azureTranslationConfig = azureTranslationConfig;
            _languageDetector = languageDetector;
            _apiHelper = apiHelper;
        }

        public async UniTask<string> TranslateTextAsync(
            string text,
            SystemLanguage targetLanguage,
            CancellationToken token)
        {
            var normalizedText = text?.Trim() ?? string.Empty;
            var languageCode = targetLanguage.SystemLanguageToISO1();

            if (normalizedText.IsValid() is false)
                return string.Empty;

            var url = _azureTranslationConfig.GetApiUrl(languageCode);
            var requestBody = new[] { new TranslationRequest(normalizedText) };

            var response =
                await _apiHelper.PostAsync<AzureTranslator, TranslationRequest[], TranslationResponse[]>(
                    this,
                    requestBody,
                    url,
                    token,
                    static (self, request) => self.SetAzureHeaders(request));

            if (response is null || response.Length == 0)
                return normalizedText;

            var firstResult = response[0];
            if (firstResult?.Translations is null || firstResult.Translations.Length == 0)
                return normalizedText;

            return firstResult.Translations[0].Text;
        }

        public UniTask<string> TranslateTextAsync(string text, CancellationToken token)
        {
            var language = _languageDetector.DetectLanguage(text);
            var oppositeLanguage = _languageSettingsRepository.GetOppositeLanguage(language);
            return TranslateTextAsync(text, oppositeLanguage, token);
        }

        private void SetAzureHeaders(UnityWebRequest request)
        {
            request.SetRequestHeader("Ocp-Apim-Subscription-Key", _azureTranslationConfig.SubscriptionKey);
            request.SetRequestHeader("Ocp-Apim-Subscription-Region", _azureTranslationConfig.Region);
        }
    }
}