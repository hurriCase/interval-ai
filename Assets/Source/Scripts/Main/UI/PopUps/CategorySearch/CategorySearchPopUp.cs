using Cysharp.Threading.Tasks;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.CategorySearch
{
    internal sealed class CategorySearchPopUp : PopUpBase
    {
        [SerializeField] private CategorySearch _categorySearch;

        internal override void Init()
        {
            _categorySearch.Init();
        }

        internal override async UniTask ShowAsync()
        {
            await base.ShowAsync();

            _categorySearch.SelectInput();
        }
    }
}