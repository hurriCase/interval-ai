using Client.Scripts.MVC.Base;
using UnityEngine;

namespace Client.Scripts.Core.MVC.Progress
{
    internal class ProgressView : MonoBehaviour, IView<ProgressModel>
    {
        [SerializeField] private Transform _progressContainer;
        [SerializeField] private GameObject _progressItemPrefab;

        private ProgressController _controller;

        internal void Init(ProgressController controller) => _controller = controller;

        public void UpdateView(ProgressModel model)
        {
            var item = Instantiate(_progressItemPrefab, _progressContainer);
            if (item.TryGetComponent<ProgressItemView>(out var progressItem) is false)
                return;

            progressItem.Init(model, _controller);
        }

        public void ClearView()
        {
            foreach (Transform child in _progressContainer)
                Destroy(child.gameObject);
        }
    }
}