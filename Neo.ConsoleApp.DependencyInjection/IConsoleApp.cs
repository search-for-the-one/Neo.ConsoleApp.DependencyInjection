using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Neo.ConsoleApp.DependencyInjection
{
    public interface IConsoleApp
    {
        Task Run();
    }

    public class ConsoleApp : IDisposable
    {
        private readonly IServiceScope scope;

        public ConsoleApp(IServiceScope scope)
        {
            this.scope = scope;
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }

    public interface IConsoleApp<TResult>
    {
        Task<TResult> Run();
    }
}