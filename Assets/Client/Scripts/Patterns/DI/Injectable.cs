namespace Client.Scripts.Patterns.DI
{
    internal abstract class Injectable
    {
        protected Injectable() => DependencyInjector.InjectDependencies(this);
    }
}