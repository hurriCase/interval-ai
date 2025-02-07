using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Client.Scripts.DB.Data;
using Client.Scripts.DB.DataRepositories.Cloud;
using CustomClasses.Runtime.Singletons;
using DependencyInjection.Runtime.InjectableMarkers;
using DependencyInjection.Runtime.InjectionBase;
using UnityEngine;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    internal sealed class EntityValidationController : Singleton<EntityValidationController>, IInjectable,
        IEntityValidationController
    {
        [Inject] private ICloudRepository _cloudRepository;

        public EntityValidationController()
        {
            InjectDependencies();
        }

        private Dictionary<string, List<ValidationRule>> EntityRules
        {
            get => EntityValidationConfig.Instance.EntityRules;
            set => EntityValidationConfig.Instance.EntityRules = value;
        }

        public async Task LoadEntityValidationRulesAsync()
        {
            try
            {
                EntityRules =
                    await _cloudRepository.ReadDataAsync<Dictionary<string, List<ValidationRule>>>(
                        DataType.Configs,
                        DBConfig.Instance.ValidationRulesPath)
                    ?? new Dictionary<string, List<ValidationRule>>();
            }
            catch (Exception e)
            {
                Debug.LogError($"[GeminiAPI::LoadChatHistoryAsync] Failed to load chat history: {e.Message}");
                EntityRules = new Dictionary<string, List<ValidationRule>>();
            }
        }

        public async Task SaveEntityValidationRulesAsync()
        {
            try
            {
                await _cloudRepository.WriteDataAsync(DataType.Configs, DBConfig.Instance.ValidationRulesPath,
                    EntityRules);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GeminiAPI::SaveChatHistoryAsync] Failed to save chat history: {e.Message}");
            }
        }


        public ValidationResult ValidateEntityContent<TContent>(string entityName, TContent entityContent)
        {
            var validationResult = new ValidationResult();
            EntityRules.TryGetValue(entityName, out var rules);

            if (rules == null)
                return validationResult;

            var properties = typeof(TContent).GetProperties();
            foreach (var property in properties)
            {
                var validation = property.GetCustomAttribute<ValidationAttribute>();
                if (validation is null)
                    continue;

                foreach (var rule in rules)
                {
                    if (property.Name != rule.PropertyName)
                        continue;

                    var (isValid, error) = rule.ValidationType switch
                    {
                        ValidationType.Required => ValidateRequired(property),
                        ValidationType.StringLength => ValidateStringLength(property, rule),
                        ValidationType.NumericRange => ValidateNumericRange(property, rule),
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    if (isValid is false)
                        validationResult.Errors.Add(error);
                }
            }

            return validationResult;
        }

        private static (bool isValid, string error) ValidateRequired(object value) =>
            value switch
            {
                null => (false, "Value cannot be null"),
                string str when string.IsNullOrWhiteSpace(str) => (false, "Value cannot be empty or whitespace"),
                _ => (true, null)
            };

        private (bool isValid, string error) ValidateStringLength(object value, ValidationRule rule)
        {
            if (value is not string str)
                return (false, "Value must be a string");

            var min = Convert.ToInt32(rule.Parameters["min"]);
            var max = Convert.ToInt32(rule.Parameters["max"]);

            if (str.Length < min)
                return (false, $"Length must be at least {min}");

            if (str.Length > max)
                return (false, $"Length must be at most {max}");

            return (true, null);
        }

        private (bool isValid, string error) ValidateNumericRange(object value, ValidationRule rule)
        {
            if (!IsNumeric(value))
                return (false, "Value must be numeric");

            var doubleValue = Convert.ToDouble(value);
            var min = Convert.ToDouble(rule.Parameters["min"]);
            var max = Convert.ToDouble(rule.Parameters["max"]);

            if (doubleValue < min)
                return (false, $"Value must be at least {min}");

            if (doubleValue > max)
                return (false, $"Value must be at most {max}");

            return (true, null);
        }

        private static bool IsNumeric(object value) =>
            value is sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal;

        public void InjectDependencies()
        {
            DependencyInjector.InjectDependencies(this);
        }
    }
}