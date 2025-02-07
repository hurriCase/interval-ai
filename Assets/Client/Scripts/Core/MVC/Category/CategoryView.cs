using Client.Scripts.Core.MVC.Base;
using UnityEngine;

namespace Client.Scripts.Core.MVC.Category
{
    internal sealed class CategoryView : MonoBehaviour, IView<CategoryModel>
    {
        [SerializeField] private Transform _categoryContainer;
        [SerializeField] private GameObject _categoryItemPrefab;

        private CategoryController _controller;

        internal void Init(CategoryController controller) => _controller = controller;

        public void UpdateView(CategoryModel model)
        {
            var item = Instantiate(_categoryItemPrefab, _categoryContainer);
            if (item.TryGetComponent<CategoryItemView>(out var categoryItem) is false)
                return;

            categoryItem.Init(model, _controller);
        }

        public void ClearView()
        {
            foreach (Transform child in _categoryContainer)
                Destroy(child.gameObject);
        }
    }
}