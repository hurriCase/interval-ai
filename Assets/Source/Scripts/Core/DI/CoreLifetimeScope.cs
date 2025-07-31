using System.Collections.Generic;
using Source.Scripts.Core.DI.StartUp;
using Source.Scripts.Core.Input;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Scenes;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Core.DI
{
    internal class CoreLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<StepBase> _stepsList;

        [SerializeField] private SceneReferences _sceneReferences;
        [SerializeField] private InputActionAsset _uiInputActionAsset;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
            builder.RegisterInstance(_sceneReferences).As<ISceneReferences>();

            builder.RegisterInstance(_stepsList);

            builder.Register<InputSystemUI>(Lifetime.Singleton);
            builder.Register<SwipeInputService>(Lifetime.Singleton).As<ISwipeInputService>();

            builder.Register<AddressablesLoader>(Lifetime.Singleton).As<IAddressablesLoader>();

            builder.RegisterEntryPoint<CoreEntryPoint>();
        }
    }
}