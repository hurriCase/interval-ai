using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Threading.Tasks;
using R3.Triggers;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    internal abstract class TransitionPracticeModuleBase<TUIBehaviour> : PracticeModule
        where TUIBehaviour : UIBehaviour
    {
        [SerializeField] protected TransitionData<TUIBehaviour>[] transitionData;

        private IModuleStateFactory _moduleStateFactory;

        [Inject]
        internal void Inject(IModuleStateFactory moduleStateFactory)
        {
            _moduleStateFactory = moduleStateFactory;
        }

        private PracticeState _practiceState;

        public override void Init(PracticeState practiceState)
        {
            _practiceState = practiceState;

            foreach (var transition in transitionData)
            {
                transition.TransitionObject.OnPointerClickAsObservable()
                    .SubscribeUntilDestroy(this, transition.ModuleType,
                        static (moduleType, self) => self.SwitchModule(moduleType));
            }
        }

        protected virtual UniTask SwitchModule(ModuleType moduleType)
        {
            var moduleStateService = _moduleStateFactory.GetOrCreate(_practiceState);
            moduleStateService.SetState(moduleType);

            return UniTask.CompletedTask;
        }
    }
}