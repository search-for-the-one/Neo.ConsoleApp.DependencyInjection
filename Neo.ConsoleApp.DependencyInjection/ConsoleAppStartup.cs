using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Neo.ConsoleApp.DependencyInjection
{
    public abstract class ConsoleAppStartup : ConsoleAppStartupBase
    {
        protected ConsoleAppStartup(params string[] args) : base(args)
        {
        }

        public async Task RunAsync<TApp>() where TApp : class, IConsoleApp
        {
            using (var serviceProvider = Configure<TApp>())
            {
                await serviceProvider.GetRequiredService<TApp>().Run();
            }
        }

        public void Run<TApp>() where TApp : class, IConsoleApp
        {
            using (var serviceProvider = Configure<TApp>())
            {
                serviceProvider.GetRequiredService<TApp>().Run().Wait();
            }
        }
    }

    public abstract class ConsoleAppStartup<TResult> : ConsoleAppStartupBase
    {
        protected ConsoleAppStartup(params string[] args) : base(args)
        {
        }

        public async Task<TResult> RunAsync<TApp>() where TApp : class, IConsoleApp<TResult>
        {
            using (var serviceProvider = Configure<TApp, TResult>())
            {
                return await serviceProvider.GetRequiredService<TApp>().Run();
            }
        }

        public TResult Run<TApp>() where TApp : class, IConsoleApp<TResult>
        {
            using (var serviceProvider = Configure<TApp, TResult>())
            {
                return serviceProvider.GetRequiredService<TApp>().Run().Result;
            }
        }
    }
}