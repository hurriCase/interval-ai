using CustomUtils.Runtime.CustomBehaviours;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.UI.Theme.Components;
using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.PopUps.Chat.Behaviours
{
    internal sealed class MessageItem : RectTransformBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private ButtonComponent _pinButton;
        [SerializeField] private ButtonComponent _translateButton;
        [SerializeField] private ButtonComponent _audioButton;

        [SerializeField] private RectTransform _messageContainer;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private ImageThemeComponent _backgroundImageTheme;

        [SerializeField] private EnumArray<MessageSourceType, MessageData> _messageSprite = new(EnumMode.SkipFirst);
        [SerializeField] private MessageSourceMapping _messageSourceMapping;

        [SerializeField] private RectTransform _controlButtonsContainer;
        [SerializeField] private float _referenceHeight;

        [SerializeField] private float _parentReferenceWidthSize;
        [SerializeField] private float _otherContentWidthSize;
        [SerializeField] private float _maxXAnchorValue;
        [SerializeField] private float _minXAnchorValue;
        [SerializeField] private float _defaultTextSize;

        [SerializeField] private float _otherContentHeightSize;

        internal void Init(RectTransform parent, string message, MessageSourceType sourceType)
        {
            _messageText.text = message;

            var parentSize = parent.rect.width;
            var differenceRatio = parentSize / _parentReferenceWidthSize;

            _messageText.fontSize = _defaultTextSize * differenceRatio;

            _controlButtonsContainer.sizeDelta = new Vector2(0, _referenceHeight * differenceRatio);

            SetMessageAnchors(differenceRatio, parentSize, sourceType);
            SetParentHeight(differenceRatio);

            _backgroundImage.sprite = _messageSprite[sourceType].BackgroundImage;
            _messageSourceMapping.SetComponentForState(sourceType, _backgroundImageTheme);
        }

        private void SetMessageAnchors(float differenceRatio, float parentSize, MessageSourceType sourceType)
        {
            _messageText.ForceMeshUpdate();

            var actualOtherSize = _otherContentWidthSize * differenceRatio;
            var totalMessageSize = _messageText.preferredWidth + actualOtherSize;
            var requiredAnchor = totalMessageSize / parentSize;
            requiredAnchor = Mathf.Clamp(requiredAnchor, _minXAnchorValue, _maxXAnchorValue);

            switch (_messageSprite[sourceType].DirectionType)
            {
                case DirectionType.Left:
                    _messageContainer.anchorMax = new Vector2(requiredAnchor, 1f);
                    break;

                case DirectionType.Right:
                    _messageContainer.anchorMin = new Vector2(1 - requiredAnchor, 0f);
                    break;
            }
        }

        private void SetParentHeight(float differenceRatio)
        {
            var actualOtherHeight = _otherContentHeightSize * differenceRatio;

            var requiredHeight = actualOtherHeight + _messageText.preferredHeight;

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, requiredHeight);
        }
    }
}