using Cysharp.Text;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    internal sealed class MaxLinesLimiter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private int _maxLines;

        private const string Ellipsis = "...";

        internal bool IsTruncated => _text.textInfo.lineCount > _maxLines;

        internal void SetText(string text)
        {
            _text.text = text;
            _text.ForceMeshUpdate();

            if (_text.textInfo.lineCount <= _maxLines)
                return;

            TruncateToMaxLines();
        }

        private void TruncateToMaxLines()
        {
            var lastLine = _text.textInfo.lineInfo[_maxLines - 1];
            var lastCharIndex = lastLine.lastCharacterIndex;

            var truncatedText = _text.text[..(lastCharIndex - Ellipsis.Length)];
            _text.SetText(ZString.Concat(truncatedText, Ellipsis));
        }
    }
}