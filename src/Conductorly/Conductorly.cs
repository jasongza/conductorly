using Conductorly.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Conductorly
{
    public sealed class Conductorly : ConductorlyBase, IConductorly
    {
        private readonly IServiceScopeFactory scopeFactory;

        public Conductorly(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public ICommandBuilder<TRequest> With<TRequest>(TRequest command) where TRequest : ICommand
        {
            return new CommandBuilder<TRequest>(command, scopeFactory);
        }
    }
}
