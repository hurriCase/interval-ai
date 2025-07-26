using System.Collections.Generic;

namespace Source.Scripts.Data.Repositories.Categories.Base
{
    internal interface IDefaultCategoriesDatabase
    {
        List<CategoryEntry> Categories { get; }
    }
}