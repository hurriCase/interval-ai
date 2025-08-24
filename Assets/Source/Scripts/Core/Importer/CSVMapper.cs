using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Source.Scripts.Core.Importer.Base;
using Source.Scripts.Core.Importer.CSVEntry;
using UnityEngine;
using ZLinq;

namespace Source.Scripts.Core.Importer
{
    internal sealed class CSVMapper : ICSVMapper
    {
        private const char CollectionDelimiter = ';';
        private const char PropertyDelimiter = '.';

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
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .ToDictionary(propertyInfo => propertyInfo.Name, StringComparer.OrdinalIgnoreCase);

        private T MapRowToObject<T>(
            IReadOnlyList<string> headerNames,
            IReadOnlyList<string> rowValues,
            Dictionary<string, PropertyInfo> propertyMap) where T : new()
        {
            var requestedObject = new T();

            var complexPropertyData = new Dictionary<string, Dictionary<string, List<string>>>();

            for (var i = 0; i < headerNames.Count && i < rowValues.Count; i++)
            {
                var headerName = headerNames[i];
                var cellValue = rowValues[i];

                if (string.IsNullOrWhiteSpace(cellValue))
                    continue;

                if (headerName.Contains(PropertyDelimiter))
                    HandleComplexProperty(complexPropertyData, headerName, cellValue);
                else
                    HandleRegularProperty(requestedObject, propertyMap, headerName, cellValue);
            }

            ProcessComplexPropertyData(requestedObject, propertyMap, complexPropertyData);

            return requestedObject;
        }

        private void HandleComplexProperty(
            Dictionary<string, Dictionary<string, List<string>>> complexPropertyData,
            string headerName,
            string cellValue)
        {
            var parts = headerName.Split(PropertyDelimiter, 2);
            var propertyName = parts[0];
            var subPropertyName = parts[1];

            if (complexPropertyData.ContainsKey(propertyName) is false)
                complexPropertyData[propertyName] = new Dictionary<string, List<string>>();

            if (complexPropertyData[propertyName].ContainsKey(subPropertyName) is false)
                complexPropertyData[propertyName][subPropertyName] = new List<string>();

            var items = cellValue.Split(CollectionDelimiter, StringSplitOptions.RemoveEmptyEntries);
            complexPropertyData[propertyName][subPropertyName].AddRange(items.Select(item => item.Trim()));
        }

        private void HandleRegularProperty(
            object targetObject,
            Dictionary<string, PropertyInfo> propertyMap,
            string headerName,
            string cellValue)
        {
            if (propertyMap.TryGetValue(headerName, out var property) is false)
                return;

            try
            {
                var convertedValue = ConvertValue(cellValue, property.PropertyType);
                property.SetValue(targetObject, convertedValue);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CSVMapper::HandleRegularProperty] " +
                               $"Failed to convert '{cellValue}' for '{headerName}': {ex.Message}");
            }
        }

        private void ProcessComplexPropertyData(
            object targetObject,
            Dictionary<string, PropertyInfo> propertyMap,
            Dictionary<string, Dictionary<string, List<string>>> complexPropertyData)
        {
            foreach (var (propertyName, subProperties) in complexPropertyData)
            {
                if (propertyMap.TryGetValue(propertyName, out var property) is false)
                    continue;

                try
                {
                    var propertyValue = CreateComplexPropertyValue(property.PropertyType, subProperties);
                    property.SetValue(targetObject, propertyValue);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[CSVMapper::ProcessComplexPropertyData] " +
                                   $"Failed to create property '{propertyName}': {ex.Message}");
                }
            }
        }

        private object CreateComplexPropertyValue(Type propertyType, Dictionary<string, List<string>> subProperties)
        {
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                return CreateStructList(propertyType, subProperties);

            if (propertyType.IsValueType && propertyType.IsPrimitive is false && propertyType.IsEnum is false)
                return CreateSingleStruct(propertyType, subProperties);

            throw new ArgumentException($"Unsupported complex property type: {propertyType}");
        }

        private object CreateSingleStruct(Type structType, Dictionary<string, List<string>> subProperties)
        {
            var structInstance = Activator.CreateInstance(structType);
            var structPropertyMap = GetStructPropertyMap(structType);

            foreach (var (propertyName, values) in subProperties)
            {
                if (structPropertyMap.TryGetValue(propertyName, out var structProperty) is false)
                    continue;

                try
                {
                    if (structProperty.PropertyType.IsGenericType &&
                        structProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var listValue = ConvertToStringList(values);
                        structProperty.SetValue(structInstance, listValue);
                    }
                    else if (values.Count > 0)
                    {
                        var convertedValue = ConvertSingleValue(values[0], structProperty.PropertyType);
                        structProperty.SetValue(structInstance, convertedValue);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[CSVMapper::CreateSingleStruct] " +
                                   $"Failed to set property '{propertyName}': {ex.Message}");
                }
            }

            return structInstance;
        }

        private object CreateStructList(Type listType, Dictionary<string, List<string>> subProperties)
        {
            var structType = listType.GetGenericArguments()[0];
            var listInstance = Activator.CreateInstance(listType);
            var addMethod = listType.GetMethod("Add");

            var maxItems = subProperties.Values.Max(list => list.Count);

            for (var i = 0; i < maxItems; i++)
            {
                var structInstance = Activator.CreateInstance(structType);
                var structPropertyMap = GetStructPropertyMap(structType);

                foreach (var (propertyName, values) in subProperties)
                {
                    if (i >= values.Count || string.IsNullOrWhiteSpace(values[i]))
                        continue;

                    if (structPropertyMap.TryGetValue(propertyName, out var structProperty) is false)
                        continue;

                    try
                    {
                        if (structProperty.PropertyType.IsGenericType &&
                            structProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>) &&
                            structProperty.PropertyType.GetGenericArguments()[0] == typeof(string))
                        {
                            var nativesValue = values[i];
                            var nativesList = nativesValue
                                .Split(CollectionDelimiter, StringSplitOptions.RemoveEmptyEntries)
                                .AsValueEnumerable()
                                .Select(s => s.Trim())
                                .Where(s => !string.IsNullOrEmpty(s))
                                .ToList();
                            structProperty.SetValue(structInstance, nativesList);
                        }
                        else
                        {
                            var convertedValue = ConvertSingleValue(values[i], structProperty.PropertyType);
                            structProperty.SetValue(structInstance, convertedValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[CSVMapper::CreateStructList] " +
                                       $"Failed to set property '{propertyName}': {ex.Message}");
                    }
                }

                addMethod?.Invoke(listInstance, new[] { structInstance });
            }

            return listInstance;
        }

        private List<string> ConvertToStringList(List<string> values)
        {
            return values.Where(v => !string.IsNullOrWhiteSpace(v)).ToList();
        }

        private Dictionary<string, PropertyInfo> GetStructPropertyMap(Type structType) =>
            structType
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .ToDictionary(propertyInfo => propertyInfo.Name, StringComparer.OrdinalIgnoreCase);

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

            if (targetType.IsEnum)
                return Enum.Parse(targetType, value, true);

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>))
                return ConvertToList(value, targetType);

            return Convert.ChangeType(value, targetType);
        }

        private object ConvertToList(string value, Type listType)
        {
            var elementType = listType.GetGenericArguments()[0];
            var listInstance = Activator.CreateInstance(listType);
            var addMethod = listType.GetMethod("Add");

            if (string.IsNullOrWhiteSpace(value))
                return listInstance;

            var elements = value.Split(CollectionDelimiter, StringSplitOptions.RemoveEmptyEntries);

            foreach (var element in elements)
            {
                var trimmedElement = element.Trim();
                if (string.IsNullOrEmpty(trimmedElement))
                    continue;

                try
                {
                    var convertedElement = ConvertSingleValue(trimmedElement, elementType);
                    addMethod?.Invoke(listInstance, new[] { convertedElement });
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[CSVMapper::ConvertToList] " +
                                   $"Failed to convert list element '{trimmedElement}' to {elementType}: {ex.Message}");
                }
            }

            return listInstance;
        }

        private object ConvertSingleValue(string value, Type targetType)
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