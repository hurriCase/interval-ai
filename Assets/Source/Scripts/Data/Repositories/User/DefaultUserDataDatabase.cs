using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.Data.Repositories.Vocabulary.CooldownSystem;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class DefaultUserDataDatabase : ScriptableObject, IDefaultUserDataDatabase
    {
        [field: SerializeField] public List<CooldownByDate> DefaultCooldowns { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}