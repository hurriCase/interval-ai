using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems
{
    internal interface IAdditionalInfoItemBase<in TTranslation> where TTranslation : ITranslation
    {
        void Init(TTranslation translation);
    }
}