﻿using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Nano.Config
{
    /// <summary>
    /// Config Manager.
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// Path.
        /// </summary>
        public static string Path { get; }

        /// <summary>
        /// Environment.
        /// </summary>
        public static string Environment { get; }

        /// <summary>
        /// Has Db-Context.
        /// </summary>
        public static bool HasDbContext { get; set; }

        /// <summary>
        /// Constructor (static).
        /// </summary>
        static ConfigManager()
        {
            ConfigManager.Path = Directory.GetCurrentDirectory();
            ConfigManager.Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        }

        /// <summary>
        /// Builds the <see cref="IConfiguration"/>.
        /// </summary>
        /// <returns>The <see cref="IConfiguration"/>.</returns>
        public static IConfiguration BuildConfiguration(params string[] args)
        {
            const string NAME = "appsettings";

            var path = ConfigManager.Path;
            var environment = ConfigManager.Environment;

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile($"{NAME}.json", false, true)
                .AddJsonFile($"{NAME}.{environment}.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            if (environment == "Development")
            {
                var tempConfiguration = configurationBuilder.Build();
                var entryPoint = tempConfiguration.GetValue<string>("App:EntryPoint");

                if (entryPoint != null)
                {
                        var workDir = Directory.GetCurrentDirectory();
                        var entryPointFile = Directory.GetFiles(workDir, entryPoint, SearchOption.AllDirectories).FirstOrDefault();

                        if (entryPointFile != null)
                        {
                            configurationBuilder
                                .AddUserSecrets(Assembly.LoadFile(entryPointFile));
                        }
                }
            }

            return configurationBuilder
                .Build();
        }
    }
}