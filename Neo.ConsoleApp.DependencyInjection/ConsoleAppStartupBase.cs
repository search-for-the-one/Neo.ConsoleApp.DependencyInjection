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
        private const string AppSettingsJson = "appsettings.json";
        private const LogLevel MinLogLevel = LogLevel.Information;

        protected ConsoleAppStartupBase(string[] args) => this.args = args;

        protected IConfigurationRoot Configuration { get; private set; }

        private void BaseConfigure(IConfigurationBuilder config)
        {
            config
                .SetBasePath(GetEntryDirectory().FullName)
                .AddJsonFile(AppSettingsJson, true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            Configure(config);
        }

        protected virtual void Configure(IConfigurationBuilder config)
        {
        }


        private void BaseConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions()
                .AddLogging(BaseConfigureLogging)
                .Configure<LoggerFilterOptions>(options => options.MinLevel = MinLogLevel);

            ConfigureServices(services);
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        private void BaseConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder
                .AddConsole()
                .AddDebug();

            ConfigureLogging(loggingBuilder);
        }

        protected virtual void ConfigureLogging(ILoggingBuilder loggingBuilder)
        {
        }

        protected ServiceProvider Configure<TApp>() where TApp : class, IConsoleApp =>
            Configure(services => services.AddSingleton<TApp>());

        protected ServiceProvider Configure<TApp, TResult>() where TApp : class, IConsoleApp<TResult> =>
            Configure(services => services.AddSingleton<TApp>());

        private ServiceProvider Configure(Action<IServiceCollection> registerConsoleApp)
        {
            if(Configuration is null)
            {
                var configurationBuilder = new ConfigurationBuilder();
                BaseConfigure(configurationBuilder);
                Configuration = configurationBuilder.Build();
            }

            var services = new ServiceCollection();
            registerConsoleApp(services);
            BaseConfigureServices(services);

            return services.BuildServiceProvider();
        }

        private static DirectoryInfo GetEntryDirectory() =>
            new FileInfo(new Uri(Assembly.GetEntryAssembly().GetName().CodeBase).AbsolutePath).Directory;
    }
}