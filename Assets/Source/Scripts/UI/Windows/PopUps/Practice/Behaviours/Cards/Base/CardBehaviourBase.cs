using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Cards.LearningComplete;
using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Modules;
using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Modules.Base;
using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Modules.Selection;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Cards.Base
{
    internal class CardBehaviourBase : MonoBehaviour
    {
        [SerializeField] protected LearningCompleteBehaviourBase learningCompleteBehaviour;
        [SerializeField] protected GameObject cardContainer;

        [SerializeField] private EnumArray<ModuleType, PracticeModuleBase> _practiceModules = new(EnumMode.SkipFirst);
        [SerializeField] private InputModuleBehaviour _inputModuleBehaviour;
        [SerializeField] private SelectionModuleBehaviour _selectionModuleBehaviour;
        [SerializeField] private AnswerModuleBehaviour _answerModuleBehaviour;

        [SerializeField] protected ControlButtonsBehaviour controlButtonsBehaviour;

        public WordEntry CurrentWord { get; protected set; }

        internal void Init()
        {
            controlButtonsBehaviour.Init(this);

            learningCompleteBehaviour.Init();
            learningCompleteBehaviour.SetActive(false);

            foreach (var module in _practiceModules)
                module.Init();

            OnInit();

            UpdateWord();
        }

        internal virtual void OnInit() { }

        internal virtual void UpdateWord() { }

        internal void SwitchModule(ModuleType moduleType)
        {
            foreach (var (type, module) in _practiceModules.AsTuples())
            {
                module.SetCurrentWord(CurrentWord);
                module.SetActive(type == moduleType);
            }
        }
    }
}