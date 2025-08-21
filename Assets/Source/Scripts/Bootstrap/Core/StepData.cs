namespace Source.Scripts.Bootstrap.Core
{
    internal readonly struct StepData
    {
        internal int Step { get; }
        internal string StepName { get; }

        internal StepData(int step, string stepName)
        {
            Step = step;
            StepName = stepName;
        }
    }
}