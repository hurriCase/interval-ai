using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client.Scripts.DB.DataRepositories.Cloud
{
    internal static class FirestoreUtils
    {
        internal static Dictionary<string, object> ToFirestoreDictionary<T>(T data)
        {
            var json = JsonConvert.SerializeObject(data);

            if (typeof(T).IsArray)
            {
                var wrappedData = new Dictionary<string, object>
                {
                    { "items", JsonConvert.DeserializeObject<object[]>(json) }
                };
                return wrappedData;
            }

            var jObject = JToken.Parse(json);
            if (jObject is JObject obj)
                return ConvertToDictionary(obj);

            return new Dictionary<string, object> { { "data", jObject } };
        }

        private static Dictionary<string, object> ConvertToDictionary(JObject jObject)
        {
            var result = new Dictionary<string, object>();

            foreach (var prop in jObject)
            {
                var value = ConvertTokenToFirestoreValue(prop.Value);
                if (value != null)
                    result[prop.Key] = value;
            }

            return result;
        }

        private static object ConvertTokenToFirestoreValue(JToken token)
        {
            return token.Type switch
            {
                JTokenType.Object => ConvertToDictionary((JObject)token),
                JTokenType.Array => token.ToObject<object[]>(),
                JTokenType.Integer => token.Value<long>(),
                JTokenType.Float => token.Value<double>(),
                JTokenType.Boolean => token.Value<bool>(),
                JTokenType.Date => token.Value<DateTime>(),
                JTokenType.String => token.Value<string>(),
                JTokenType.Null => null,
                _ => token.Value<string>()
            };
        }
    }
}