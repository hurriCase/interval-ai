using System;
using System.Threading.Tasks;

namespace Client.Scripts.Core.StartUp
{
    internal interface IStep
    {
        event Action<int, string> OnStepCompleted;
        Task Execute(int step);
    }
}