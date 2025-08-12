using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Category
{
    internal sealed class WordItem : MonoBehaviour
    {
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;
        [SerializeField] private TextMeshProUGUI _learningWordText;
        [SerializeField] private TextMeshProUGUI _translationsText;

        internal void Init(WordEntry wordEntry)
        {
            _learningWordText.text = wordEntry.LearningWord;
            _translationsText.text = wordEntry.NativeWord;

            _wordProgressBehaviour.Init();
            _wordProgressBehaviour.UpdateProgress(wordEntry);
        }
    }
}