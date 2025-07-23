using Source.Scripts.UI.Windows;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.UI.DI
{
    internal sealed class UILifetimeScope : LifetimeScope
    {
        [SerializeField] private WindowsController _windowsController;
        [SerializeField] private MenuBehaviour _menuBehaviour;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).As<IWindowsController>();
            builder.RegisterComponent(_menuBehaviour).As<IMenuBehaviour>();

            builder.RegisterEntryPoint<UIEntryPoint>();
        }
    }
}