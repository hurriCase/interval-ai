using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.UI
{
    internal sealed class UILifetimeScope : LifetimeScope
    {
        [SerializeField] private WindowsController _windowsController;

        protected override void Configure(IContainerBuilder builder)
        {
            _windowsController.Init();
        }
    }
}