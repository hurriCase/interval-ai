using Client.Scripts.DB.Entities.Base;

namespace Client.Scripts.Core.MVC.Base
{
    internal abstract class ModelBase<TContent> where TContent : class
    {
        internal EntryData<TContent> Data { get; private set; }

        protected ModelBase(EntryData<TContent> data) => Data = data;
    }
}