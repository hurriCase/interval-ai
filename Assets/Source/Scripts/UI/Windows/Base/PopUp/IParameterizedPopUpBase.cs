namespace Source.Scripts.UI.Windows.Base.PopUp
{
    internal interface IParameterizedPopUpBase<in TParameters>
    {
        void SetParameters(TParameters parameters);
    }
}