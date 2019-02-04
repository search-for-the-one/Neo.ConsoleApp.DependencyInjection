using System.Threading.Tasks;

namespace Neo.ConsoleApp.DependencyInjection
{
    public abstract class ConsoleAppStartup : ConsoleAppStartupBase
    {

        protected ConsoleAppStartup(params string[] args) : base(args)
        {
        }

        public async Task RunAsync<TApp>() where TApp : class, IConsoleApp => await Configure<TApp>().Run();

        public void Run<TApp>() where TApp : class, IConsoleApp => Configure<TApp>().Run().Wait();
    }

    public abstract class ConsoleAppStartup<TResult> : ConsoleAppStartupBase
    {
        protected ConsoleAppStartup(params string[] args) : base(args)
        {
        }

        public async Task<TResult> RunAsync<TApp>() where TApp : class, IConsoleApp<TResult> => await Configure<TApp, TResult>().Run();

        public TResult Run<TApp>() where TApp : class, IConsoleApp<TResult> => Configure<TApp, TResult>().Run().Result;
    }
}