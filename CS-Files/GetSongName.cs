using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer.CS_Files
{
    public class GetSongName
    {
        public static string GetNameOfSong(in int index) 
        {
            SongList songList = new SongList();
            string[] songs = songList.GetSongList();

            for (int i = 0; i < songs.Length; i++)           
                songs[i] = songs[i].Replace('_', ' ');           

            return songs[index];
        }
    }
}
