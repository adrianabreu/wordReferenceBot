using DbUp;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace WordreferenceBot.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true);

            var configuration = builder.Build();

            var db = configuration["SqlConnectionString"];

            var upgradeEngine = DeployChanges.To
                .SqlDatabase(db)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            Console.WriteLine($"Deploying changes to {db}");
            upgradeEngine.PerformUpgrade();
            #if DEBUG
                Console.ReadKey();
            #endif
            Console.WriteLine("Finished migration");
        }
    }
}
