using System.Collections;
using System.Threading.Tasks;

namespace Client.Scripts.Patterns.Extensions
{
    internal static class ConvertExtension
    {
        internal static IEnumerator WaitForTask(this Task task)
        {
            while (task.IsCompleted is false)
            {
                yield return null;
            }

            if (task.Exception != null)
                throw task.Exception;
        }
    }
}