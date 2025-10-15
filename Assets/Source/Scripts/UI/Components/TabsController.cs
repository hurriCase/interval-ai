using System;
using System.Threading;
using CustomUtils.Runtime.Animations;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    [Serializable]
    internal sealed class TabsController<TEnum> where TEnum : unmanaged, Enum
    {
        [field: SerializeField] internal EnumArray<TEnum, ToggleComponent> Tabs { get; private set; } =
            new(EnumMode.SkipFirst);

        [SerializeField] private AnchoredPositionAnimation<TEnum> _tabAnimation;

        private TEnum _currentState;

        internal void Init(TEnum initialState, CancellationToken token)
        {
            foreach (var (state, tab) in Tabs.AsTuples())
            {
                tab.OnPointerClickAsObservable()
                    .Subscribe((self: this, state), static (_, tuple) => tuple.self.SwitchState(tuple.state))
                    .RegisterTo(token);
            }

            SwitchState(initialState);
        }

        internal void SwitchState(TEnum state, bool isInstant = false)
        {
            _tabAnimation.PlayAnimation(state, isInstant);
            Tabs[state].isOn = true;
        }
    }
}