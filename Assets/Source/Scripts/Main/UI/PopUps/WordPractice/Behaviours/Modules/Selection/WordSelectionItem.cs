using System.Collections.Generic;
using CustomUtils.Runtime.UI.CustomComponents.ProceduralUIImage;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles.Mappings;
using Source.Scripts.Core.Localization.Translator.Translations;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Selection
{
    internal sealed class WordSelectionItem : UIBehaviour
    {
        [SerializeField] private StateToggle _toggle;
        [SerializeField] private ProceduralImage _checkedImage;
        [SerializeField] private float _incorrectBorderWidth;

        [SerializeField] private List<ToggleGraphicMapping> _correctMapping;
        [SerializeField] private List<ToggleGraphicMapping> _incorrectMapping;

        private IPracticeSettingsRepository _practiceSettingsRepository;

        [Inject]
        internal void Inject(IPracticeSettingsRepository practiceSettingsRepository)
        {
            _practiceSettingsRepository = practiceSettingsRepository;
        }

        internal void Init(WordEntry wordEntry, bool isCorrect)
        {
            _toggle.Text.text = wordEntry.Word.GetHiddenText(_practiceSettingsRepository);
            _checkedImage.BorderWidth.Value = isCorrect ? 0 : _incorrectBorderWidth;
            _toggle.AdditionalGraphics = isCorrect ? _correctMapping : _incorrectMapping;
            _toggle.isOn = false;
        }

        internal void SeActive()
        {
            _toggle.isOn = true;
        }
    }
}