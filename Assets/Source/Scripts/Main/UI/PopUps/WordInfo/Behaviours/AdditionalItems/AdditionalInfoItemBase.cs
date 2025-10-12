using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems
{
    internal abstract class AdditionalInfoItemBase<TTranslation> : MonoBehaviour where TTranslation : ITranslation
    {
        internal virtual void Init() { }
        internal abstract void UpdateView(TTranslation translation);
    }
}