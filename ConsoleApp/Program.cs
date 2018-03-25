using System;
using System.Collections.Generic;
using System.IO;
using MediaOrganiserCore;
using MediaOrganiserCore.Implementations;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
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
                Imp3Manager mgr = new Mp3Manager();
                var mp3s = mgr.GetAllMp3s(rootFolder, true);
				Console.WriteLine("{0} MP3s found.", mp3s.Count);
				Console.WriteLine();
                foreach(var mp3 in mp3s)
				{
					Console.WriteLine(mgr.CleanMp3(mp3));
				}
            }

            Console.WriteLine("DONE. PRESS ANY KEY TO EXIT.");
            Console.ReadLine();
        }
    }
}
