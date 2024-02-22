using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AudioPlayer.CS_Files
{
    public class MusicPlayer
    {
        //public static FileInfo[] GetFiles() => MusicDirectory.GetFilesInMusicDirectory();

        public string[] GetSongs(FileInfo[] files)
        {
            string[] sounds = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
                sounds[i] = "MusicList\\" + files[i].Name;

            return sounds;
        }
        
        public int Play(int currentsongId, string[] songs, MediaPlayer audioPlayer) 
        {           
            audioPlayer.Open(new Uri(songs[currentsongId], UriKind.Relative));

            audioPlayer.Play();

            return currentsongId;
        }

        public int Play(int currentsongId, string[] songs, MediaPlayer audioPlayer, TimeSpan position)
        {

            audioPlayer.Open(new Uri(songs[currentsongId], UriKind.Relative));
            audioPlayer.Position = position;

            audioPlayer.Play();

            return currentsongId;
        }

        public int PlayNext(int currentsongId, string[] songs, MediaPlayer audioPlayer)
        {

            if (currentsongId == songs.Length - 1)
                currentsongId = Play(0, songs, audioPlayer);
            else
                currentsongId = Play(++currentsongId, songs, audioPlayer);

            if (currentsongId > songs.Length)
                currentsongId = songs.Length - 1;
            

            return currentsongId;
        }

        public int PlayPrevios(int currentsongId, string[] songs, MediaPlayer audioPlayer)
        {

            if (currentsongId == 0)
                currentsongId = Play(songs.Length - 1, songs, audioPlayer);
            else
                currentsongId = Play(--currentsongId, songs, audioPlayer);

            if (currentsongId < 0)
                currentsongId = 0;
            

            return currentsongId;
        }
    }
}
