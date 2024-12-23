using Client.Scripts.MVC.Base;
using UnityEngine;

namespace Client.Scripts.Core.MVC.Word
{
    internal class WordView : MonoBehaviour, IView<WordModel>
    {
        [SerializeField] private Transform _wordContainer;
        [SerializeField] private GameObject _wordItemPrefab;

        private WordController _controller;

        internal void Init(WordController controller)
        {
            _controller = controller;
        }

        public void UpdateView(WordModel model)
        {
            var item = Instantiate(_wordItemPrefab, _wordContainer);
            if (item.TryGetComponent<WordItemView>(out var wordItem) is false)
                return;

            wordItem.Init(model, _controller);
        }

        public void ClearView()
        {
            foreach (Transform child in _wordContainer)
                Destroy(child.gameObject);
        }
    }
}