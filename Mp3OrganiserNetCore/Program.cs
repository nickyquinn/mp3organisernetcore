using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mp3OrganiserNetCore
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            List<string> lsArtists = new List<string>();
            List<string> lsInvalidArtistFiles = new List<string>();

            //var rootFolder = args[0];
            var rootFolder = @"/home/nick/Music/";
            //var dirs = Directory.GetDirectories(rootFolder);
            Console.WriteLine("GETTING ARTISTS...");

            var files = Directory.GetFiles(rootFolder, "*.mp3", SearchOption.AllDirectories);
            var fileCount = files.Count();
            var currentCount = 0;
            foreach(var file in files)
            {
                currentCount++;
                var mp3File = TagLib.File.Create(file);
                var artists = mp3File.Tag.Performers;
                foreach(var artist in artists)
                {
                    var misplacedThe = (artist.Contains(",The") || artist.Contains(", The") || artist.Contains("(The)"));

                    if (!lsArtists.Contains(artist) && misplacedThe)
                    {
                        lsArtists.Add(artist);
                    }
                    if(misplacedThe)
                    {
                        //Console.WriteLine(file);
                        lsInvalidArtistFiles.Add(file);
                    }
                }
                Console.Write("\r{0} of {1}", currentCount, fileCount);
            }

            foreach (string artist in lsArtists.OrderBy(x => x).ToList())
            {
                Console.WriteLine(artist);
            }
            Console.WriteLine("ALL FILES SEARCHED.");
            Console.WriteLine("PRESS ANY KEY TO AUTO REPAIR FILES");
            Console.ReadLine();

            foreach (string mp3File in lsInvalidArtistFiles)
            {
                var fileToEdit = TagLib.File.Create(mp3File);
                var artists = fileToEdit.Tag.Performers;
                bool fileDidNeedChanging = false;
                for(int i = 0;i < artists.Count();i++)
                {
                    var thisArtist = fileToEdit.Tag.Performers[i];
                    var misplacedThe = (thisArtist.Contains(",The") || thisArtist.Contains(", The") || thisArtist.Contains("(The)"));
                    if(misplacedThe)
                    {
                        thisArtist = thisArtist.Replace(",The", "").Replace(", The", "").Replace("(The)", "");
                        thisArtist = "The " + thisArtist.Trim();
                        artists[i] = thisArtist;
                        fileToEdit.Tag.Performers = artists;
                        fileDidNeedChanging = true;
                        //fileToEdit.Save();
                    }
                }

                if(fileDidNeedChanging)
                {
                    try
                    {
                        fileToEdit.Save();
                        Console.WriteLine("SAVED {0}", mp3File);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Could not save file {0}", mp3File);
                    }
                }
            }

            Console.WriteLine("DONE. PRESS ANY KEY TO EXIT.");
            Console.ReadLine();
        }
    }
}