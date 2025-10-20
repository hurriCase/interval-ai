using System.Runtime.CompilerServices;
using ZLinq;

[assembly: InternalsVisibleTo("Source.Scripts.Bootstrap")]
[assembly: InternalsVisibleTo("Source.Scripts.Data")]
[assembly: InternalsVisibleTo("Source.Scripts.UI")]
[assembly: InternalsVisibleTo("Source.Scripts.Main")]
[assembly: InternalsVisibleTo("Source.Scripts.Onboarding")]
[assembly: InternalsVisibleTo("Source.Scripts.Editor")]

[assembly: ZLinqDropIn("Source.Scripts.Core",
    DropInGenerateTypes.Array | DropInGenerateTypes.List | DropInGenerateTypes.Enumerable)]