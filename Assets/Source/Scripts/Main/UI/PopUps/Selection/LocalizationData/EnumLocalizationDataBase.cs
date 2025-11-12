using System;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Selection.LocalizationData
{
    internal abstract class EnumLocalizationDataBase : ScriptableObject
    {
        protected const string LocalizationsPath = "Localization Config/";

        internal abstract string GetLocalization<TEnumParameter>(TEnumParameter currentEnum)
            where TEnumParameter : unmanaged, Enum;
    }
}