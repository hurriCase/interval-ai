using System.Threading.Tasks;

namespace Client.Scripts.DB.Entities.Base.Validation
{
    internal interface IEntityValidationController
    {
        ValidationResult ValidateEntityContent<TContent>(string entityName, TContent entityContent);
        Task LoadEntityValidationRulesAsync();

        Task SaveEntityValidationRulesAsync();
    }
}