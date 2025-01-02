using System.IO;

namespace AudioPlayer.CS_Files
{
    public class SongList
    {
        private static string[] _SongsList { get; set; }
     
        public static string[] GetSongList()
        {
            FileInfo[] musics = MusicDirectory.GetFilesInMusicDirectory();

            _SongsList = new string[musics.Length];

            for (int j = 0; j < musics.Length; j++)
            {
                _SongsList[j] = musics[j].Name;
                _SongsList[j] = _SongsList[j].Replace('_', ' ');
            }      
                        
            return _SongsList;
        }
    }    
}
