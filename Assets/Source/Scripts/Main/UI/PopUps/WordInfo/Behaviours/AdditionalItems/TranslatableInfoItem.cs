using Source.Scripts.Core.Repositories.Words.Word;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems
{
    internal class TranslatableInfoItem : MonoBehaviour, IAdditionalInfoItemBase<Translation>
    {
        [SerializeField] private TextMeshProUGUI _learningText;
        [SerializeField] private TextMeshProUGUI _nativeText;

        public void Init() { }

        public void UpdateView(Translation translationSet)
        {
            _learningText.text = translationSet.Learning;
            _nativeText.text = translationSet.Native;
        }
    }
}