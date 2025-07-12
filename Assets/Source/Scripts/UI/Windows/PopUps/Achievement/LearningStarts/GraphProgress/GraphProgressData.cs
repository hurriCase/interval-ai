namespace Source.Scripts.UI.Windows.PopUps.Achievement.LearningStarts.GraphProgress
{
    internal readonly struct GraphProgressData
    {
        internal int Index { get; }
        internal int Progress { get; }

        internal GraphProgressData(int index, int progress)
        {
            Index = index;
            Progress = progress;
        }

        public void Deconstruct(out int index, out int progress)
        {
            index = Index;
            progress = Progress;
        }
    }
}