using Cysharp.Text;
using UnityEngine;

namespace Source.Scripts.Core.Audio.AudioRecord
{
    internal static class AndroidJavaObjectExtensions
    {
        internal static void CallWithLog(
            this AndroidJavaObject javaObject,
            string methodName,
            params object[] args)
        {
            javaObject.Call(methodName, args);
            AndroidLogger.Log(ZString.Concat(methodName, ZString.Join(", ", args)));
        }
    }
}