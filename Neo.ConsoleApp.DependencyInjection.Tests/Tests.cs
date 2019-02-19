using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Neo.ConsoleApp.DependencyInjection.Tests
{
    public class Tests
    {
        [Test]
        public void Run() => new Startup().Run<ConsoleApp>();

        [Test]
        public async Task RunAsync() => await new Startup().RunAsync<ConsoleApp>();

        [Test]
        public void RunGeneric() => Assert.AreEqual(1, new GenericStartup().Run<GenericConsoleApp>());

        [Test]
        public async Task RunAsyncGeneric() => Assert.AreEqual(1, await new GenericStartup().RunAsync<GenericConsoleApp>());
    }

    public class ConsoleApp : IConsoleApp
    {
        private readonly IMock mock;

        public ConsoleApp(IMock mock) => this.mock = mock;

        public Task Run()
        {
            Assert.AreEqual(1, mock.Value);
            return Task.CompletedTask;
        }
    }

    public class GenericConsoleApp : IConsoleApp<int>
    {
        private readonly IMock mock;

        public GenericConsoleApp(IMock mock) => this.mock = mock;

        public Task<int> Run() => Task.FromResult(mock.Value);
    }

    public class Startup : ConsoleAppStartup
    {
        protected override void ConfigureServices(IServiceCollection services) => services.AddSingleton<IMock, Mock>();
    }

    public class GenericStartup : ConsoleAppStartup<int>
    {
        protected override void ConfigureServices(IServiceCollection services) => services.AddSingleton<IMock, Mock>();
    }

    public interface IMock
    {
        int Value { get; }
    }

    public class Mock : IMock
    {
        public int Value { get; } = 1;
    }
}
