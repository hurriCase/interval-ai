using CustomUtils.Runtime.Animations;
using CustomUtils.Runtime.Attributes;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    [UsedImplicitly]
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class NotificationComponent : MonoBehaviour, INotificationComponent
    {
        [SerializeField, Self] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _messageText;

        [SerializeField] private AlphaAnimation<VisibilityState> _alphaAnimation;
        [SerializeField] private float _durationPerCharacter = 0.05f;

        private void Awake()
        {
            _alphaAnimation.PlayAnimation(VisibilityState.Hidden, true);
        }

        public async UniTask ShowMessage(string message)
        {
            _messageText.SetText(message);

            await _alphaAnimation.PlayAnimation(VisibilityState.Visible);

            var duration = _messageText.text.Length * _durationPerCharacter;
            await UniTask.WaitForSeconds(duration);

            HideMessage();
        }

        public void HideMessage()
        {
            _alphaAnimation.PlayAnimation(VisibilityState.Hidden);
        }
    }
}