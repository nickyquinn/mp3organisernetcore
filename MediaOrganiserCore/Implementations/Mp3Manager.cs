using System;
using System.Collections.Generic;
using System.IO;

namespace MediaOrganiserCore.Implementations
{
	public class Mp3Manager : Imp3Manager
    {
		public Mp3Manager()
        {
        }

		public List<string> GetAllMp3s(string directory, bool includeSubDirs)
        {
            var files = Directory.GetFiles(directory, "*.mp3", includeSubDirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            return new List<string>(files);
        }

		public string CleanMp3(string filePath)
		{
			var changes = new List<string>();
			TagLib.File fileToEdit = null;
			try
			{
				fileToEdit = TagLib.File.Create(filePath);
			}
            catch(Exception)
			{
				return filePath + " - ERR: NOT VALID MP3";
			}
            var artists = fileToEdit.Tag.Performers;
			for(int i = 0;i < artists.Length;i++)
            {
                var thisArtist = fileToEdit.Tag.Performers[i];
				var cleanedArtist = CleanArtist(thisArtist);
				if(thisArtist != cleanedArtist)
                {
					artists[i] = cleanedArtist;
                    fileToEdit.Tag.Performers = artists;
					changes.Add("Artist: " + thisArtist + " -> " + cleanedArtist);
                }
            }
            
			if(changes.Count > 0)
            {
                try
                {
                    fileToEdit.Save();
					return filePath + " - FIXED: " + string.Join(", ", changes);
                }
                catch(Exception)
                {
					return filePath + " - ERR: could not update MP3";
                }
			} else {
				return filePath + " - OK";
			}
		}

		private string CleanArtist(string artist)
		{
			var misplacedThe = (artist.Contains(",The") || artist.Contains(", The") || artist.Contains("(The)"));
			if(misplacedThe)
			{
				artist = "The" + artist.Replace(",The", "").Replace(", The", "").Replace("(The)", "");
			}
			return artist;
		}

		private string MakeValidFileName(string name)
        {
           string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
           string invalidRegStr = string.Format( @"([{0}]*\.+$)|([{0}]+)", invalidChars );

           return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "");
        }

	}
}
