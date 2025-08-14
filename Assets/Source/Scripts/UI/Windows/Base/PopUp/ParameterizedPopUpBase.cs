namespace Source.Scripts.UI.Windows.Base.PopUp
{
    internal class ParameterizedPopUpBase<TParameters> : PopUpBase
    {
        protected TParameters Parameters { get; private set; }

        internal virtual void SetParameters(TParameters parameters)
        {
            Parameters = parameters;
        }
    }
}