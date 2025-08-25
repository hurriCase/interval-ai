namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress
{
    internal readonly struct DescriptionData
    {
        internal string Title { get; }
        internal string Description { get; }
        internal int Percent { get; }

        internal DescriptionData(string title, string description, int percent)
        {
            Title = title;
            Description = description;
            Percent = percent;
        }
    }
}