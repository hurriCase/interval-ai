using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using R3;
using R3.Triggers;
using Source.Scripts.UI.Components;
using UnityEngine;

namespace Source.Scripts.Onboarding.UI.PopUp.WordPractice
{
    [Serializable]
    internal sealed class PlainStep : PracticeStepBase
    {
        [SerializeField] private List<ButtonComponent> _switchButtons;

        private readonly List<IDisposable> _subscriptions = new();

        private Transform _tintParent;

        internal override void Init(Transform tintParent, CancellationToken cancellationToken)
        {
            _tintParent = tintParent;

            foreach (var buttonComponent in _switchButtons)
            {
                var subscription = buttonComponent.OnPointerClickAsObservable()
                    .Subscribe(buttonClickSubject, static (_, clickSubject) => clickSubject.OnNext(Unit.Default));

                _subscriptions.Add(subscription);
                subscription.RegisterTo(cancellationToken);
            }
        }

        internal override void ActiveStep()
        {
            _tintParent.SetActive(false);
        }

        internal override void HideStep()
        {
            foreach (var subscription in _subscriptions)
                subscription?.Dispose();

            _subscriptions.Clear();
        }
    }
}