using System.Threading.Tasks;

namespace Neo.ConsoleApp.DependencyInjection
{
    public interface IConsoleApp
    {
        Task Run();
    }

    public interface IConsoleApp<TResult>
    {
        Task<TResult> Run();
    }
}