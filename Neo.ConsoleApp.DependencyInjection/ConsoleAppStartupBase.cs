using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Neo.ConsoleApp.DependencyInjection
{
    public abstract class ConsoleAppStartupBase
    {
        private readonly string[] args;

        protected ConsoleAppStartupBase(string[] args) => this.args = args;

        protected IConfigurationRoot Configuration { get; private set; }
        protected IServiceProvider ServiceProvider { get; private set; }
        protected ILogger Logger { get; private set; }

        protected virtual void Configure(IConfigurationBuilder config)
        {
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        protected virtual void ConfigureLogging(ILoggerFactory loggerFactory)
        {
        }

        protected TApp Configure<TApp>() where TApp : class, IConsoleApp =>
            Configure(services => services.AddSingleton<TApp>()).GetRequiredService<TApp>();

        protected TApp Configure<TApp, TResult>() where TApp : class, IConsoleApp<TResult> =>
            Configure(services => services.AddSingleton<TApp>()).GetRequiredService<TApp>();

        private IServiceProvider Configure(Action<IServiceCollection> registerConsoleApp)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .SetBasePath(GetEntryDirectory().FullName)
                .AddJsonFile("appsettings.json", true, true);

            Configure(configurationBuilder);

            Configuration = configurationBuilder.Build();

            var services = new ServiceCollection()
                .AddOptions()
                .AddLogging();

            registerConsoleApp(services);

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();

            ConfigureLogging(loggerFactory);

            Logger = loggerFactory.CreateLogger(nameof(ConsoleAppStartup));

            return ServiceProvider;
        }

        private static DirectoryInfo GetEntryDirectory() =>
            new FileInfo(new Uri(Assembly.GetEntryAssembly().GetName().CodeBase).AbsolutePath).Directory;
    }
}