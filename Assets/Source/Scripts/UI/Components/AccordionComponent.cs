using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    internal sealed class AccordionComponent : MonoBehaviour
    {
        [field: SerializeField] internal ButtonComponent ExpandButton { get; private set; }
        [field: SerializeField] internal List<GameObject> HiddenContent { get; private set; }

        internal bool IsExpanded { get; private set; }

        private void Awake()
        {
            ExpandButton.OnClickAsObservable()
                .Subscribe(this,
                    static (_, component) => component.SwitchContent(component.IsExpanded is false))
                .RegisterTo(destroyCancellationToken);
        }

        internal void SwitchContent(bool isExpanded)
        {
            foreach (var hidden in HiddenContent)
                hidden.SetActive(isExpanded);

            IsExpanded = isExpanded;
        }
    }
}