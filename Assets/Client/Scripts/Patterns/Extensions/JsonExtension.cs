using System;
using System.Linq;
using Newtonsoft.Json;

namespace Client.Scripts.Patterns.Extensions
{
    internal static class JsonExtension
    {
        public static string GetJsonPropertyName(this Enum enumValue)
        {
            var attribute = enumValue.GetType()
                .GetField(enumValue.ToString())
                ?.GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                .FirstOrDefault() as JsonPropertyAttribute;

            return attribute?.PropertyName ?? enumValue.ToString();
        }
    }
}