using CustomUtils.Runtime.Extensions;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress
{
    internal readonly struct DescriptionData
    {
        internal string Title { get; }
        internal string Description { get; }
        internal int Percent { get; }

        internal DescriptionData(ProgressLocalizationData localizationData, int percent)
        {
            Title = localizationData.TitleKey.GetLocalization();
            Description = localizationData.ProgressDescriptionKey.GetLocalization();
            Percent = percent;
        }
    }
}