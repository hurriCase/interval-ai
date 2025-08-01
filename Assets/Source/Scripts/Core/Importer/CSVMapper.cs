using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Source.Scripts.Core.Importer.Base;
using Source.Scripts.Core.Importer.CSVEntry;
using UnityEngine;

namespace Source.Scripts.Core.Importer
{
    internal sealed class CSVMapper : ICSVMapper
    {
        public T[] MapToObjects<T>(CSVTable csvTable) where T : new()
        {
            var propertyMap = GetPropertyMap<T>();
            var headerNames = csvTable.Header.Values;
            var objects = new T[csvTable.Rows.Length];

            for (var i = 0; i < csvTable.Rows.Length; i++)
                objects[i] = MapRowToObject<T>(headerNames, csvTable.Rows[i].Values, propertyMap);

            return objects;
        }

        private Dictionary<string, PropertyInfo> GetPropertyMap<T>() =>
            typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(propertyInfo => propertyInfo.CanWrite)
                .ToDictionary(propertyInfo => propertyInfo.Name, StringComparer.OrdinalIgnoreCase);

        private T MapRowToObject<T>(
            IReadOnlyList<string> headerNames,
            IReadOnlyList<string> rowValues,
            Dictionary<string, PropertyInfo> propertyMap) where T : new()
        {
            var requestedObject = new T();

            for (var i = 0; i < headerNames.Count && i < rowValues.Count; i++)
            {
                var headerName = headerNames[i];
                var cellValue = rowValues[i];

                if (propertyMap.TryGetValue(headerName, out var property) is false)
                    continue;

                if (string.IsNullOrWhiteSpace(cellValue))
                    continue;

                try
                {
                    var convertedValue = ConvertValue(cellValue, property.PropertyType);
                    property.SetValue(requestedObject, convertedValue);
                }
                catch (Exception ex)
                {
                    Debug.LogError("[GenericCSVMapper::MapRowToObject] " +
                                   $"Failed to convert '{cellValue}' for '{headerName}': {ex.Message}");
                }
            }

            return requestedObject;
        }

        private object ConvertValue(string value, Type targetType)
        {
            if (targetType == typeof(string))
                return value;

            if (targetType == typeof(int))
                return int.Parse(value);

            if (targetType == typeof(bool))
                return bool.Parse(value);

            if (targetType == typeof(DateTime))
                return DateTime.Parse(value);

            return targetType.IsEnum
                ? Enum.Parse(targetType, value, true)
                : Convert.ChangeType(value, targetType);
        }
    }
}