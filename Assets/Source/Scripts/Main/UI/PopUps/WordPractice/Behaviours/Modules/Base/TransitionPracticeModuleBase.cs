using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Threading.Tasks;
using R3.Triggers;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.ModuleState;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    internal abstract class TransitionPracticeModuleBase<TUIBehaviour> : PracticeModule
        where TUIBehaviour : UIBehaviour
    {
        [SerializeField] protected TransitionData<TUIBehaviour>[] transitionData;

        private IModuleStateServiceFactory _moduleStateServiceFactory;

        [Inject]
        internal void Inject(IModuleStateServiceFactory moduleStateServiceFactory)
        {
            _moduleStateServiceFactory = moduleStateServiceFactory;
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
            _moduleStateServiceFactory.GetOrCreate(_practiceState).SetState(moduleType);

            return UniTask.CompletedTask;
        }
    }
}