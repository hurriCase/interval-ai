using System;
using UnityEngine;

namespace Client.Scripts.Core.Localization
{
    [Serializable]
    internal class Sheet
    {
        public string Name;
        public long Id;
        public TextAsset TextAsset;
    }
}