using System;
using UnityEngine;

namespace Assets.SimpleLocalization.Scripts
{
    [Serializable]
    internal class Sheet
    {
        public string Name;
        public long Id;
        public TextAsset TextAsset;
    }
}