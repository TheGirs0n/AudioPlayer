using AudioPlayer.CS_Files;
using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int currentsongId;
        MusicPlayer audioPlayer;
        MediaPlayer mediaPlayer;
        string[] songs;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSound();
            InitializeSongs();
        }

        public void InitializeSound()
        {
            audioPlayer = new MusicPlayer();
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Volume = VolumeSlider.Value;
        }

        public void InitializeSongs() => songs = audioPlayer.GetSongs(audioPlayer.GetFiles());
        

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            currentsongId = audioPlayer.Play(currentsongId, songs, mediaPlayer);
            SongParametrs();           
        }

        private void AddNewSong_Click(object sender, RoutedEventArgs e)
        {
            AddNewSongs newSongs = new AddNewSongs();
            if(newSongs.GetFiles().Length > 0)
                newSongs.SaveFiles(newSongs.GetFiles());        
        }

        private void SongList_Click(object sender, RoutedEventArgs e) 
        {
            SongList songList = new SongList();
            string[] songs = songList.GetSongList();

            for (int i = 0; i < songs.Length; i++)
                MessageBox.Show(songs[i]);
        }

        private void NextSong_Click(object sender, RoutedEventArgs e)
        {
            currentsongId = audioPlayer.PlayNext(currentsongId, songs, mediaPlayer);
            SongParametrs();
        }

        private void PreviosSong_Click(object sender, RoutedEventArgs e)
        {
            currentsongId = audioPlayer.PlayPrevios(currentsongId, songs, mediaPlayer);
            SongParametrs();
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)VolumeSlider.Value / 100;
        }

        void SongParametrs()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            //MusicPlayer.InitializeTimeLine(mediaPlayer);
            SongName.Text = GetSongName.GetNameOfSong(currentsongId);
            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                StartTimer.Text = mediaPlayer.Position.ToString(@"mm\:ss");
                EndTimer.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            }
            else
                SongName.Text = "No song selected...";
        }

    }
}
