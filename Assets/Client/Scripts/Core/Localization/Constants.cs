using System.Collections.Generic;

namespace Client.Scripts.Core.Localization
{
    internal static class Constants
    {
        internal const string SheetResolverUrl =
            "https://script.google.com/macros/s/AKfycbycW2dsGZhc2xJh2Fs8yu9KUEqdM-ssOiK1AlES3crLqQa1lkDrI4mZgP7sJhmFlGAD/exec";

        internal const string TableUrlPattern = "https://docs.google.com/spreadsheets/d/{0}";

        internal static readonly Dictionary<string, int> ExampleSheets = new()
            { { "Menu", 0 }, { "Settings", 331980525 }, { "Tests", 1674352817 } };
    }
}