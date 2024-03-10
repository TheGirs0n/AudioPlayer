using AudioPlayer.CS_Files;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSound();
            InitializeSongs();
            LoadSongList();
        }

        public void InitializeSound()
        {
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            audioPlayer = new MusicPlayer();
            mediaPlayer = new MediaPlayer();
            VolumeSlider.Value = 50;
            mediaPlayer.Volume = VolumeSlider.Value;
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
        }

        public void InitializeSongs() => _songs = audioPlayer.GetSongs(MusicDirectory.GetFilesInMusicDirectory());
        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {          
            MusicSlider.IsEnabled = true;
            MusicSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            MusicSlider.Visibility = Visibility.Visible;
            MusicSlider.TickFrequency = 60 / mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
        }

        void LoadSongList()
        {
            string[] _songs = SongList.GetSongList();
            for (int i = 0; i < _songs.Length; i++)
                ListOfSongs.Items.Add(_songs[i]);
        }

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

            ChangePlayerStatus(PlayerStatus.Play);
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
            if (ListOfSongs.Visibility == Visibility.Visible)
                ListOfSongs.Visibility = Visibility.Hidden;
            else if(ListOfSongs.Visibility == Visibility.Hidden)
                ListOfSongs.Visibility = Visibility.Visible;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                StartTimer.Text = mediaPlayer.Position.ToString(@"mm\:ss");
                EndTimer.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");

                //MusicSlider.Value += 0.5;
                MusicSlider.Value += 1;
            }
            else
                SongName.Text = "No song selected...";

            if (mediaPlayer.NaturalDuration.HasTimeSpan && (mediaPlayer.Position == mediaPlayer.NaturalDuration.TimeSpan))
            {
                try
                {
                    if (statusSong == StatusSong.RepeatSong)
                    {
                        currentsongId = audioPlayer.Play(currentsongId, _songs, mediaPlayer);
                        SongParametrs();
                    }
                    else if (statusSong == StatusSong.RepeatPlaylist)
                    {
                        currentsongId = audioPlayer.PlayNext(currentsongId, _songs, mediaPlayer);
                        SongParametrs();
                    }
                    else if (statusSong == StatusSong.Standart)
                    {
                        if (currentsongId == _songs.Length - 1)
                        {
                            mediaPlayer.Close();
                            SongName.Text = "Playlist is over";

                            ChangePlayerStatus(PlayerStatus.Play);
                        }
                        else
                        {
                            currentsongId = audioPlayer.PlayNext(currentsongId, _songs, mediaPlayer);
                            SongParametrs();
                        }
                    }
                    else if (statusSong == StatusSong.Random)
                    {
                        Random rnd = new Random();
                        int newsongId;

                        do
                        {
                            newsongId = rnd.Next(0, _songs.Length);
                        }
                        while (newsongId == currentsongId);

                        currentsongId = audioPlayer.Play(newsongId, _songs, mediaPlayer);
                        SongParametrs();
                    }
                }
                catch(Exception ex) {MessageBox.Show(ex.Message); }
            }
        }
        /// <summary>
        /// Настройки песни, вкл. при новой
        /// </summary>

        private void SongParametrs()
        {          
            timer.Interval = TimeSpan.FromSeconds(1);

            if(!timer.IsEnabled)
                timer.Start();
           
            MusicSlider.Value = 0;
            SongName.Text = GetSongName.GetNameOfSong(currentsongId);           
        }
        private void RepeatStatus_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage[] imageSource = new BitmapImage[4]
            {
                new BitmapImage(new Uri("/Pictures/fast-forward.png",UriKind.Relative)),
                new BitmapImage(new Uri("/Pictures/repeatPlaylist.png",UriKind.Relative)),
                new BitmapImage(new Uri("/Pictures/repeatSong.png",UriKind.Relative)),
                new BitmapImage(new Uri("/Pictures/random.png",UriKind.Relative))
            };

            if (statusSong != StatusSong.Random)
                UpdateStatusSong(++statusSong, imageSource[(int)statusSong]);
            else
                UpdateStatusSong(StatusSong.Standart, imageSource[0]);        
        }

        /// <summary>
        /// Изменение статуса повтора
        /// </summary>
        /// <param name="newsongStatus"></param>
        void UpdateStatusSong(StatusSong newsongStatus, BitmapImage bitmapImage)
        {
            RepeatImage.Source = bitmapImage;
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
        private void MusicSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
            StartTimer.Text = mediaPlayer.Position.ToString(@"mm\:ss");
            //currentPosition = TimeSpan.FromSeconds(MusicSlider.Value);

            ChangePlayerStatus(PlayerStatus.Pause);
        }

        private void MusicSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            timer.Start();
            currentPosition = TimeSpan.FromSeconds(MusicSlider.Value);
            currentsongId = audioPlayer.Play(currentsongId, _songs, mediaPlayer, currentPosition);
            StartTimer.Text = mediaPlayer.Position.ToString(@"mm\:ss");

            ChangePlayerStatus(PlayerStatus.Play);
        }

        private void SongsList_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ListOfSongs.SelectedItem != null)
            {
                var li = (string)ListOfSongs.Items[ListOfSongs.SelectedIndex];
                MessageBox.Show((string)li);

                currentsongId = audioPlayer.Play(ListOfSongs.SelectedIndex, _songs, mediaPlayer);
                SongParametrs();
            }
        }
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)VolumeSlider.Value / 100;
        }
    }

    public enum StatusSong
    {
        Standart = 0,
        RepeatPlaylist = 1,
        RepeatSong = 2,
        Random = 3
    }
    public enum PlayerStatus
    {
        Play = 0,
        Pause = 1
    }
}
