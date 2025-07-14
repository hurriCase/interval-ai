using R3;
using R3.Triggers;
using Source.Scripts.Core.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Selectables
{
    internal sealed class SwitchableToggle : Toggle
    {
        [field: SerializeField] internal GameObject CheckedObject { get; private set; }
        [field: SerializeField] internal GameObject UncheckedObject { get; private set; }

        protected override void Reset()
        {
            base.Reset();

            transition = Transition.None;
        }

        protected override void Awake()
        {
            base.Awake();

            this.OnPointerClickAsObservable()
                .Subscribe(static _ => AudioHandler.Instance.PlayOneShotSound(SoundType.Button))
                .RegisterTo(destroyCancellationToken);

            this.OnValueChangedAsObservable()
                .Subscribe(this, static (isOn, toggle) =>
                {
                    if (!toggle.CheckedObject || !toggle.UncheckedObject)
                        return;

                    toggle.CheckedObject.SetActive(isOn);
                    toggle.UncheckedObject.SetActive(!isOn);
                })
                .RegisterTo(destroyCancellationToken);
        }
    }
}