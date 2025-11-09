using System;
using System.Threading;
using CustomUtils.Runtime.Animations;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    [Serializable]
    internal sealed class TabsController<TEnum> where TEnum : unmanaged, Enum
    {
        [field: SerializeField] internal EnumArray<TEnum, TabItem> Tabs { get; private set; }

        [SerializeField] private AnchoredPositionAnimation<TEnum> _tabAnimation;

        private TEnum _currentState;

        internal void Init(TEnum initialState, CancellationToken token)
        {
            foreach (var (state, tab) in Tabs.AsTuples())
            {
                tab.SwitchToggle.OnPointerClickAsObservable()
                    .Subscribe((self: this, state), static (_, tuple) => tuple.self.SwitchState(tuple.state))
                    .RegisterTo(token);
            }

            SwitchState(initialState);
        }

        internal void SwitchState(TEnum state, bool isInstant = false)
        {
            _currentState = state;

            var currentTab = Tabs[state];
            currentTab.CanvasGroup.Show();
            currentTab.SwitchToggle.isOn = true;

            _tabAnimation.PlayAnimation(state, isInstant)
                .OnComplete(this, static self => self.HideOtherTabs());
        }

        private void HideOtherTabs()
        {
            foreach (var tab in Tabs)
            {
                if (tab == Tabs[_currentState])
                    continue;

                tab.CanvasGroup.Hide();
            }
        }
    }
}