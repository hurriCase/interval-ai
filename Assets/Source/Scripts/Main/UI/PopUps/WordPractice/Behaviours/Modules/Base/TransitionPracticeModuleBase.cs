using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3.Triggers;
using Source.Scripts.Core.Configs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    internal abstract class TransitionPracticeModuleBase<TUIBehaviour> : PracticeModule
        where TUIBehaviour : UIBehaviour
    {
        [SerializeField] protected TransitionData<TUIBehaviour>[] transitionData;

        private CardBehaviour _cardBehaviour;

        public override void Init(CardBehaviour cardBehaviour)
        {
            _cardBehaviour = cardBehaviour;

            foreach (var transition in transitionData)
            {
                transition.TransitionObject.OnPointerClickAsObservable()
                    .SubscribeAndRegister(this, transition.ModuleType,
                        static (moduleType, self) => self.SwitchModule(moduleType));
            }
        }

        protected virtual UniTask SwitchModule(ModuleType moduleType)
        {
            _cardBehaviour.SwitchModuleCommand.Execute(moduleType);

            return UniTask.CompletedTask;
        }
    }
}