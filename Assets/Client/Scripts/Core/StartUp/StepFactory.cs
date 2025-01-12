using System;

namespace Client.Scripts.Core.StartUp
{
    internal sealed class StepFactory
    {
        internal static IStep CreateStep(Type stepType)
        {
            if (typeof(IStep).IsAssignableFrom(stepType) is false)
                throw new ArgumentException($"Type {stepType.Name} does not implement IStep");

            return Activator.CreateInstance(stepType) as IStep;
        }
    }
}