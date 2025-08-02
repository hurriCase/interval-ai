namespace Source.Scripts.Core.Repositories
{
    internal interface IDefaultEntry<TEntry>
    {
        int DefaultId { get; }
        TEntry Entry { get; }
    }
}