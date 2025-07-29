using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.Screens.Generation.Behaviours
{
    internal sealed class MessageItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private ButtonComponent _pinButton;
        [SerializeField] private ButtonComponent _translateButton;
        [SerializeField] private ButtonComponent _audioButton;

        internal void Init(string message)
        {
            _messageText.text = message;


        }
    }
}