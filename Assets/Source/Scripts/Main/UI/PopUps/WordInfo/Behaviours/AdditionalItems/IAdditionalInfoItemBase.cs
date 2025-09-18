using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems
{
    internal interface IAdditionalInfoItemBase<in TTranslation> where TTranslation : ITranslation
    {
        void Init();
        void UpdateView(TTranslation translation);
    }
}