using System;
using System.Threading.Tasks;

namespace Client.Scripts.Steps
{
    public interface IStep
    {
        event Action<int, string> OnCompleted;
        Task Execute(int step);
    }
}