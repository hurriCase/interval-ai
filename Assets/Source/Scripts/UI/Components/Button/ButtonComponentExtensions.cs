using System;
using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.UI.Windows.Base;

namespace Source.Scripts.UI.Components.Button
{
    internal static class ButtonComponentExtensions
    {
        internal static void SubscribeWithHide<TPopUp>(
            this ButtonComponent button,
            TPopUp popUp,
            Action<TPopUp> action)
            where TPopUp : PopUpBase
        {
            button.OnClickAsObservable()
                .SubscribeUntilDestroy(popUp, action, static (action, self) =>
                {
                    action.Invoke(self);
                    self.HideAsync().Forget();
                });
        }
    }
}