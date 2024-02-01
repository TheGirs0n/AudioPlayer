using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AudioPlayer.CS_Files
{
    public class MusicPlayer
    {
        public FileInfo[] GetFiles() => MusicDirectory.GetFilesInMusicDirectory();

        public static MediaTimeline InitializeTimeLine(MediaPlayer audioPlayer) => new MediaTimeline(new Uri(audioPlayer.Source.ToString()));

        public static void GetSongTimeLine(MediaPlayer audioPlayer)
        {
            var timeline = InitializeTimeLine(audioPlayer);
            var line = timeline.Duration;
        }

        public string[] GetSongs(FileInfo[] files)
        {
            string[] sounds = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
                sounds[i] = "MusicList\\" + files[i].Name;

            return sounds;
        }
        
        public int Play(int currentsongId, string[] songs, MediaPlayer audioPlayer, string status = "") 
        {           
            audioPlayer.Stop();

            audioPlayer.Open(new Uri(songs[currentsongId], UriKind.Relative));

            if (status == "Repeat" || status == "InfinityRepeat")
            {
                //var timeline = InitializeTimeLine(audioPlayer);
                //timeline.RepeatBehavior = RepeatBehavior.Forever;
                //audioPlayer.Clock = timeline.CreateClock();
                //audioPlayer.Clock.Controller.Begin();
            }
            else
                audioPlayer.Play();

            return currentsongId;
        }

        public int PlayNext(int currentsongId, string[] songs, MediaPlayer audioPlayer, string status = "")
        {
            audioPlayer.Stop();

            if (currentsongId == songs.Length - 1)
                currentsongId = Play(0, songs, audioPlayer);
            else
                currentsongId = Play(++currentsongId, songs, audioPlayer);

            if (currentsongId > songs.Length)
                currentsongId = songs.Length - 1;

            return currentsongId;
        }

        public int PlayPrevios(int currentsongId, string[] songs, MediaPlayer audioPlayer, string status = "")
        {
            audioPlayer.Stop();

            if (currentsongId == 0)
                currentsongId = Play(songs.Length - 1, songs, audioPlayer);
            else
                currentsongId = Play(--currentsongId, songs, audioPlayer);

            if (currentsongId < 0)
                currentsongId = 0;

            return currentsongId;
        }

        public void UpdatePlayStatus(string status) 
        {   
            
        }

        public string GetPlayStatus() 
        {
            string status = string.Empty;
            return status;
        }

        public void IsLooping()
        {
            //audioPlayer.Pos
              
        }
    }
}
