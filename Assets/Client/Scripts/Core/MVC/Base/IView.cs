namespace Client.Scripts.Core.MVC.Base
{
    internal interface IView<in TModel> where TModel : class
    {
        void UpdateView(TModel model);
        void ClearView();
    }
}