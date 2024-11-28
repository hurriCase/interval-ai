using System.Threading.Tasks;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;

namespace Client.Scripts.Database.Controllers
{
    internal sealed class UserEntityController : Injectable, IUserEntityController
    {
        [Inject] private IDBController _dbController;
        
        public Task Init()
        {
            throw new System.NotImplementedException();
        }
    }
    
    public interface IUserEntityController
    {
        public Task Init();
    }
}