using System.Runtime.CompilerServices;
using ZLinq;

[assembly: InternalsVisibleTo("Source.Scripts.Onboarding")]
[assembly: ZLinqDropIn("Source.Scripts.Main",
    DropInGenerateTypes.Array | DropInGenerateTypes.List | DropInGenerateTypes.Enumerable)]