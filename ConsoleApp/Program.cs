using System;
using System.Collections.Generic;
using System.IO;
using MediaOrganiserCore;
using MediaOrganiserCore.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
			// Configure DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<Imp3Manager, Mp3Manager>()
                .BuildServiceProvider();

            // Configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            // Configure Mp3 Manager
			logger.LogDebug("Configuring MP3 manager");
			var mp3Manager = serviceProvider.GetService<Imp3Manager>();

			List<string> lsArtists = new List<string>();
            List<string> lsInvalidArtistFiles = new List<string>();

            //var rootFolder = args[0];
            Console.WriteLine("Enter path to search for media:");
            var rootFolder = Console.ReadLine();
            Console.WriteLine("GETTING ARTISTS...");

            if (!Directory.Exists(rootFolder))
            {
                Console.WriteLine("NOT A DIRECTORY. EXITING...");
                Environment.Exit(0);
            }
            else
            {
				var mp3s = mp3Manager.GetAllMp3s(rootFolder, true);
				Console.WriteLine("{0} MP3s found.", mp3s.Count);
				Console.WriteLine();
                foreach(var mp3 in mp3s)
				{
					Console.WriteLine(mp3Manager.CleanMp3(mp3));
				}
            }

            Console.WriteLine("DONE. PRESS ANY KEY TO EXIT.");
            Console.ReadLine();
        }
    }
}
