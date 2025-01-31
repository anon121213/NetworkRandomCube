using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.StateMachine.StateFactory
{
    public class StatesFactory : IStatesFactory
    {
        private readonly LifetimeScope _parentScope;

        public StatesFactory(LifetimeScope parentScope)
        {
            _parentScope = parentScope;
        }

        public IExitableState CreateSystem<TState>() where TState : class, IExitableState =>
            _parentScope.CreateChild(builder =>
                builder.Register<TState>(Lifetime.Transient)).Container.Resolve<TState>();
    }

    public interface IStatesFactory
    {
        IExitableState CreateSystem<TState>() where TState : class, IExitableState;
    }
}