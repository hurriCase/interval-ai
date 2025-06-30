using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Scripts.UI
{
    [ExecuteAlways]
    internal sealed class Checkbox : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _checkedObject;
        [SerializeField] private GameObject _uncheckedObject;

        [SerializeField] private bool _isOn;

        private bool IsOn
        {
            get => _isOn;
            set
            {
                if (_isOn == value)
                    return;

                _isOn = value;
                UpdateVisualState();
            }
        }

        private void Start()
        {
            UpdateVisualState();
        }

        private void OnValidate()
        {
            UpdateVisualState();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            IsOn = IsOn is false;
        }

        private void UpdateVisualState()
        {
            if (!_uncheckedObject || !_checkedObject)
                return;

            _uncheckedObject.SetActive(_isOn is false);
            _checkedObject.SetActive(_isOn);
        }
    }
}