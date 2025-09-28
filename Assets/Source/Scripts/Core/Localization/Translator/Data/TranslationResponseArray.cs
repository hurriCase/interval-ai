using System;
using CustomUtils.Runtime.Json;
using Newtonsoft.Json;
using Source.Scripts.Core.Api.Interfaces;

namespace Source.Scripts.Core.Localization.Translator.Data
{
    [Serializable]
    [JsonConverter(typeof(ArrayConverter<TranslationResponseArray, TranslationResponse>))]
    internal sealed class TranslationResponseArray : IValidatable, IWrapper<TranslationResponse>
    {
        public TranslationResponse[] Items { get; private set; }

        public void SetItems(TranslationResponse[] items)
        {
            Items = items;
        }

        public bool IsValid() => Items is { Length: > 0 } && Items[0]?.Translations is { Length: > 0 };
        internal string GetText() => Items[0].Translations[0].Text;
    }
}