using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.Data.Repositories.Vocabulary.CooldownSystem;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.User
{
    internal interface IDefaultUserDataDatabase
    {
        List<CooldownByDate> DefaultCooldowns { get; }
        string Name { get; }
        Sprite Icon { get; }
    }
}