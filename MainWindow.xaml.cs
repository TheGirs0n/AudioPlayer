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
        TimeSpan currentPosition;
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

        /// <summary>
        /// Воспроизведение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPosition == TimeSpan.Zero)
                currentsongId = audioPlayer.Play(currentsongId, _songs, mediaPlayer);
            else
                currentsongId = audioPlayer.Play(currentsongId, _songs, mediaPlayer, currentPosition);

            SongParametrs();

            if (mediaPlayer.Source != null)
            {
                Play.Visibility = Visibility.Hidden;
                Play_Image.Visibility = Visibility.Hidden;
                Play.IsEnabled = false;

                Stop.Visibility = Visibility.Visible;
                Stop_Image.Visibility = Visibility.Visible;
                Stop.IsEnabled = true;
            }
            
        }
        /// <summary>
        /// Стоп
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

            currentPosition = mediaPlayer.Position;
            mediaPlayer.Stop();
            mediaPlayer.Position = currentPosition;

            ChangePlayerStatus(PlayerStatus.Pause);

        }
        /// <summary>
        /// Добавление новых песен (.mp3)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewSong_Click(object sender, RoutedEventArgs e)
        {
            AddNewSongs newSongs = new AddNewSongs();
            string[] songs = newSongs.GetFiles();

            if (songs.Length > 0)
                newSongs.SaveFiles(songs);

            _songs = SongList.GetSongList();
        }
        /// <summary>
        /// Список песен
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SongList_Click(object sender, RoutedEventArgs e)
        {
            SongListWindow songListWindow = new SongListWindow();
            songListWindow.Show();
        }
        /// <summary>
        /// Следующая песня с текущими настройками повтора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextSong_Click(object sender, RoutedEventArgs e)
        {
            ChangePlayerStatus(PlayerStatus.Play);
            currentsongId = audioPlayer.PlayNext(currentsongId, _songs, mediaPlayer);
            SongParametrs();
        }
        /// <summary>
        /// Предыдущая песня с текущими настройками повтора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviosSong_Click(object sender, RoutedEventArgs e)
        {
            ChangePlayerStatus(PlayerStatus.Play);
            currentsongId = audioPlayer.PlayPrevios(currentsongId, _songs, mediaPlayer);
            SongParametrs();
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)VolumeSlider.Value / 100;
        }
        /// <summary>
        /// Настройки песни, вкл. при новой
        /// </summary>
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
                StartTimer.Text = mediaPlayer.Position.ToString(@"mm\:ss");
                EndTimer.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
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
                else if (statusSong == StatusSong.RepeatPlaylist)
                {
                    currentsongId = audioPlayer.PlayNext(currentsongId, _songs, mediaPlayer);
                    SongName.Text = GetSongName.GetNameOfSong(currentsongId);
                }
                else if (statusSong == StatusSong.Standart)
                {
                    if (currentsongId == _songs.Length - 1)
                    {
                        mediaPlayer.Close();
                        SongName.Text = "Playlist is over";
                    }
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

        /// <summary>
        /// Изменение статуса повтора
        /// </summary>
        /// <param name="newsongStatus"></param>
        void UpdateStatusSong(StatusSong newsongStatus)
        {
            statusSong = newsongStatus;
            MessageBox.Show("New Repeat Status is: " + $" {statusSong}");
        }

        void ChangePlayerStatus(PlayerStatus playerStatus)
        {
            if (playerStatus == PlayerStatus.Play)
            {
                Play.Visibility = Visibility.Hidden;
                Play_Image.Visibility = Visibility.Hidden;
                Play.IsEnabled = false;

                Stop.Visibility = Visibility.Visible;
                Stop_Image.Visibility = Visibility.Visible;
                Stop.IsEnabled = true;
            }
            else
            {
                Play.Visibility = Visibility.Visible;
                Play_Image.Visibility = Visibility.Visible;
                Play.IsEnabled = true;

                Stop.Visibility = Visibility.Hidden;
                Stop_Image.Visibility = Visibility.Hidden;
                Stop.IsEnabled = false;
            }
        }

    }

    public enum StatusSong
    {
        Standart = 0,
        RepeatPlaylist = 1,
        RepeatSong = 2
    }
    public enum PlayerStatus
    {
        Play = 0,
        Pause = 1
    }
}
