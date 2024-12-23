namespace Client.Scripts.MVC.Base
{
    internal interface IView<TModel> where TModel : class
    {
        void UpdateView(TModel model);
        void ClearView();
    }
}