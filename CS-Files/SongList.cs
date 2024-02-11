using System;
using System.IO;
using System.Linq;

namespace AudioPlayer.CS_Files
{
    public class SongList
    {
        private static string[] SongsList { get; set; }

       // public void AddSongs(string[] songs) => SongsList = songs;

        public static string[] GetSongList()
        {
            //var musicDirectory = MusicDirectory.GetMusicDirectory();
            FileInfo[] musics = MusicDirectory.GetFilesInMusicDirectory();

            SongsList = new string[musics.Length];

            for (int j = 0; j < musics.Length; j++)
            {
                SongsList[j] = musics[j].Name;
                SongsList[j] = SongsList[j].Replace('_', ' ');
            }      
                        
            return SongsList;
        }
    }    
}
