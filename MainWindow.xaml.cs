using AudioPlayer.CS_Files;
using System;
using System.Diagnostics.Eventing.Reader;
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
        string[] _songs;
        StatusSong statusSong = StatusSong.Standart;

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
            VolumeSlider.Value = 50;
            mediaPlayer.Volume = VolumeSlider.Value;
            SongParametrs();
        }

        public void InitializeSongs() => _songs = audioPlayer.GetSongs(MusicDirectory.GetFilesInMusicDirectory());
        

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentsongId = audioPlayer.Play(currentsongId, _songs, mediaPlayer);
                SongParametrs();              
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddNewSong_Click(object sender, RoutedEventArgs e)
        {
            AddNewSongs newSongs = new AddNewSongs();
            string[] songs = newSongs.GetFiles();

            if(songs.Length > 0)
                newSongs.SaveFiles(songs);

            _songs = SongList.GetSongList();
        }

        private void SongList_Click(object sender, RoutedEventArgs e) 
        {           
            for (int i = 0; i < _songs.Length; i++)
                MessageBox.Show(_songs[i]);
        }

        private void NextSong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentsongId = audioPlayer.PlayNext(currentsongId, _songs, mediaPlayer);
                SongParametrs();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PreviosSong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentsongId = audioPlayer.PlayPrevios(currentsongId, _songs, mediaPlayer);
                SongParametrs();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)VolumeSlider.Value / 100;
        }

        private void SongParametrs()
        {           
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            SongName.Text = GetSongName.GetNameOfSong(currentsongId);   

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                try
                {
                    StartTimer.Text = mediaPlayer.Position.ToString(@"mm\:ss");
                    EndTimer.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                }
                catch (Exception) { }
            }
            else
                SongName.Text = "No song selected...";

            if (mediaPlayer.NaturalDuration.HasTimeSpan && (mediaPlayer.Position == mediaPlayer.NaturalDuration.TimeSpan))
            {
                if (statusSong == StatusSong.RepeatSong)
                {
                    currentsongId = audioPlayer.Play(currentsongId, _songs, mediaPlayer);
                    SongName.Text = GetSongName.GetNameOfSong(currentsongId);
                }
                else 
                {
                    currentsongId = audioPlayer.PlayNext(currentsongId, _songs, mediaPlayer);
                    SongName.Text = GetSongName.GetNameOfSong(currentsongId);
                }
            }
        }

        private void RepeatStatus_Click(object sender, RoutedEventArgs e)
        {
            if (statusSong != StatusSong.RepeatSong)
                UpdateStatusSong(++statusSong);
            else
                UpdateStatusSong(StatusSong.Standart);
        }
        

        void UpdateStatusSong(StatusSong newsongStatus)
        {
            statusSong = newsongStatus;
            MessageBox.Show("New Repeat Status is: " + $" {statusSong}");
        }
    }

    public enum StatusSong
    {
        Standart = 0, 
        RepeatPlaylist = 1,
        RepeatSong = 2
    }
}
