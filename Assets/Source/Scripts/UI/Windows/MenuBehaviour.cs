using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using R3.Triggers;
using Source.Scripts.UI.Selectables;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;

namespace Source.Scripts.UI.Windows
{
    internal sealed class MenuBehaviour : MonoBehaviour
    {
        [SerializeField] private EnumArray<ScreenType, ThemeToggle> _menuToggles = new(EnumMode.SkipFirst);

        internal void Init()
        {
            var initialScreenType = WindowsController.Instance.GetInitialScreenType();

            foreach (var (screenType, themeToggle) in _menuToggles.AsTuples())
            {
                themeToggle.OnPointerClickAsObservable()
                    .Subscribe(screenType, (_, type) => WindowsController.Instance.OpenScreenByType(type))
                    .RegisterTo(destroyCancellationToken);

                if (screenType == initialScreenType)
                    themeToggle.isOn = true;
            }
        }
    }
}