using System;
using CustomUtils.Runtime.Downloader;
using UnityEngine;

namespace Source.Scripts.Editor.DefaultDataCreation
{
    [Serializable]
    internal sealed class DefaultDataSheet : Sheet
    {
        [field: SerializeField] public DefaultDataType DefaultDataType { get; private set; }
    }
}