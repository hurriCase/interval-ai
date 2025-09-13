using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Chat
{
    internal sealed class MessageItem : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private ButtonComponent _pinButton;
        [SerializeField] private ButtonComponent _translateButton;

        [SerializeField] private float _minWidth;
        [SerializeField] private float _maxWidth;
        [SerializeField] private float _otherContentWidth;

        internal void Init(string message)
        {
            _messageText.text = message;

            AdjustMessageSize();
        }

        private void AdjustMessageSize()
        {
            var contentWidth = _messageText.preferredWidth + _otherContentWidth;
            var targetWidth = Mathf.Clamp(contentWidth, _minWidth, _maxWidth);
            _contentContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        }
    }
}