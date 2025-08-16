namespace Source.Scripts.UI.Windows.Base.PopUp
{
    internal class ParameterizedPopUpBase<TParameters> : PopUpBase, IParameterizedPopUpBase
        where TParameters : class
    {
        protected TParameters Parameters { get; private set; }

        public virtual void SetParameters(object parameters)
        {
            Parameters = parameters as TParameters;
        }
    }
}