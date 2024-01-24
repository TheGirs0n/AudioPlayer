using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AudioPlayer.CS_Files
{
    public class SongList
    {
        private string[] SongsList { get; set; }

        public void AddSongs(string[] songs) => SongsList = songs;

        public string[] GetSongList()
        {
            var musicDirectory = MusicDirectory.GetMusicDirectory();
            FileInfo[] musics = musicDirectory.GetFiles();

            SongsList = new string[musics.Length];
            for (int j = 0; j < musics.Length; j++)
                SongsList[j] = musics[j].Name;              
                        
            return SongsList;
        }
    }    
}
