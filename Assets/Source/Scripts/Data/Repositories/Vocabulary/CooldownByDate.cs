using System;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary
{
    [Serializable]
    internal struct CooldownByDate
    {
        [field: SerializeField] internal int Cooldown { get; set; }
        [field: SerializeField] internal DateType DateType { get; set; }

        internal readonly DateTime AddToDateTime(DateTime dateTime) =>
            DateType switch
            {
                DateType.Minutes => dateTime.AddMinutes(Cooldown),
                DateType.Hours => dateTime.AddHours(Cooldown),
                DateType.Days => dateTime.AddDays(Cooldown),
                DateType.Weeks => dateTime.AddDays(Cooldown * 7),
                DateType.Months => dateTime.AddMonths(Cooldown),
                DateType.Years => dateTime.AddYears(Cooldown),
                _ => dateTime
            };
    }
}