using AudioPlayer.CS_Files;
using System;
using System.IO;
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
        private MusicPlayer _audioPlayer;
        private MediaPlayer _mediaPlayer;
        private TimeSpan _currentPosition;

        private int _currentsongId;
        private string[] _songs;

        private StatusSong _statusSong = StatusSong.Standart;
        private PlayerStatus _playerStatus = PlayerStatus.Pause;

        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSound();
            InitializeSongs();
            LoadSongList();
        }

        private void InitializeSound()
        {         
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;

            _audioPlayer = new MusicPlayer();
            _mediaPlayer = new MediaPlayer();

            VolumeSlider.Value = 50;
            _mediaPlayer.Volume = VolumeSlider.Value;

            _mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
        }

        private void InitializeSongs() => _songs = _audioPlayer.GetSongs(MusicDirectory.GetFilesInMusicDirectory());

        private void LoadSongList()
        {
            string[] _songs = SongList.GetSongList();
            for (int i = 0; i < _songs.Length; i++)
                ListOfSongs.Items.Add(_songs[i]);
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {          
            MusicSlider.IsEnabled = true;
            MusicSlider.Maximum = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            MusicSlider.Visibility = Visibility.Visible;
            MusicSlider.TickFrequency = 60 / _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
        }

        /// <summary>
        /// Воспроизведение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_songs.Length != 0)
            {
                if (_currentPosition == TimeSpan.Zero)
                    _currentsongId = _audioPlayer.Play(_currentsongId, _songs, _mediaPlayer);
                else
                    _currentsongId = _audioPlayer.Play(_currentsongId, _songs, _mediaPlayer, _currentPosition);

                SongParametrs();
                MusicSlider.Value = _currentPosition.Seconds;

                ChangePlayerStatus(PlayerStatus.Play);
            }
            else
                SongName.Text = "No song to play...";
        }
        /// <summary>
        /// Стоп
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_songs.Length != 0)
            {
                _currentPosition = _mediaPlayer.Position;
                _mediaPlayer.Stop();
                _mediaPlayer.Position = _currentPosition;

                ChangePlayerStatus(PlayerStatus.Pause);
            }
            else
                SongName.Text = "No song to pause...";
        }
        /// <summary>
        /// Следующая песня с текущими настройками повтора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextSong_Click(object sender, RoutedEventArgs e)
        {
            if (_songs.Length != 0)
            {
                ChangePlayerStatus(PlayerStatus.Play);
                _currentsongId = _audioPlayer.PlayNext(_currentsongId, _songs, _mediaPlayer);

                SongParametrs();
            }
            else
                SongName.Text = "No song to go forward...";
        }
        /// <summary>
        /// Предыдущая песня с текущими настройками повтора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviosSong_Click(object sender, RoutedEventArgs e)
        {
            if (_songs.Length != 0)
            {
                ChangePlayerStatus(PlayerStatus.Play);
                _currentsongId = _audioPlayer.PlayPrevios(_currentsongId, _songs, _mediaPlayer);

                SongParametrs();
            }
            else
                SongName.Text = "No song to go back...";
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
            {
                ListOfSongs.Visibility = Visibility.Hidden;
                ListOfSongs.Items.Clear();
                this.Height = 150;
            }
            else if (ListOfSongs.Visibility == Visibility.Hidden)
            {
                LoadSongList();
                ListOfSongs.Visibility = Visibility.Visible;
                this.Height = 250;
            }
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

            if (_statusSong != StatusSong.Random)
                UpdateStatusSong(++_statusSong, imageSource[(int)_statusSong]);
            else
                UpdateStatusSong(StatusSong.Standart, imageSource[0]);        
        }

        private void MusicSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _mediaPlayer.Stop();
            timer.Stop();
           
            StartTimer.Text = _mediaPlayer.Position.ToString(@"mm\:ss");

            ChangePlayerStatus(PlayerStatus.Pause);
        }

        private void MusicSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            timer.Start();
            _currentPosition = TimeSpan.FromSeconds(MusicSlider.Value);
            _currentsongId = _audioPlayer.Play(_currentsongId, _songs, _mediaPlayer, _currentPosition);
            StartTimer.Text = _mediaPlayer.Position.ToString(@"mm\:ss");

            ChangePlayerStatus(PlayerStatus.Play);
        }

        private void SongsList_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ListOfSongs.SelectedItem != null)
            {
                _currentsongId = _audioPlayer.Play(ListOfSongs.SelectedIndex, _songs, _mediaPlayer);
                SongParametrs();
                ChangePlayerStatus(PlayerStatus.Play);
            }
        }
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _mediaPlayer.Volume = (double)VolumeSlider.Value / 100;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MusicPlayerData musicPlayerData = new MusicPlayerData(songId: _currentsongId, songName: GetSongName.GetNameOfSong(_currentsongId), songPosition: _currentPosition);
            
            musicPlayerData.SerializeJSONAsync(musicPlayerData);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("MusicData.json") == true)
            {
                MusicPlayerData musicPlayerData = new MusicPlayerData().DeserializeJSON();

                _currentsongId = musicPlayerData.SongId;
                SongName.Text = musicPlayerData.SongName;
                _currentPosition = musicPlayerData.SongPosition;
                MusicSlider.Value = _currentPosition.TotalSeconds;
            }
            else
            {
                _currentsongId = 0;
                SongName.Text = "No start song...";
                _currentPosition = new TimeSpan(0, 0, 0);
                MusicSlider.Value = 0;
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_mediaPlayer.Source != null)
            {
                StartTimer.Text = _mediaPlayer.Position.ToString(@"mm\:ss");
                EndTimer.Text = _mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");

                if (_playerStatus == PlayerStatus.Play)
                    MusicSlider.Value += 1;
            }
            else
                SongName.Text = "No song selected...";

            if (_mediaPlayer.NaturalDuration.HasTimeSpan && (_mediaPlayer.Position == _mediaPlayer.NaturalDuration.TimeSpan))
            {
                try
                {
                    if (_statusSong == StatusSong.RepeatSong)
                    {
                        _currentsongId = _audioPlayer.Play(_currentsongId, _songs, _mediaPlayer);
                        SongParametrs();
                    }
                    else if (_statusSong == StatusSong.RepeatPlaylist)
                    {
                        _currentsongId = _audioPlayer.PlayNext(_currentsongId, _songs, _mediaPlayer);
                        SongParametrs();
                    }
                    else if (_statusSong == StatusSong.Standart)
                    {
                        if (_currentsongId == _songs.Length - 1)
                        {
                            _mediaPlayer.Close();
                            SongName.Text = "Playlist is over";

                            ChangePlayerStatus(PlayerStatus.Play);
                        }
                        else
                        {
                            _currentsongId = _audioPlayer.PlayNext(_currentsongId, _songs, _mediaPlayer);
                            SongParametrs();
                        }
                    }
                    else if (_statusSong == StatusSong.Random)
                    {
                        Random rnd = new Random();
                        int newsongId;

                        do
                        {
                            newsongId = rnd.Next(0, _songs.Length);
                        }
                        while (newsongId == _currentsongId);

                        _currentsongId = _audioPlayer.Play(newsongId, _songs, _mediaPlayer);
                        SongParametrs();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        /// <summary>
        /// Настройки песни, вкл. при новой
        /// </summary>
        private void SongParametrs()
        {
            timer.Interval = TimeSpan.FromSeconds(1);

            if (!timer.IsEnabled)
                timer.Start();

            MusicSlider.Value = 0;
            SongName.Text = GetSongName.GetNameOfSong(_currentsongId);
        }

        /// <summary>
        /// Изменение статуса повтора
        /// </summary>
        /// <param name="newsongStatus"></param>
        private void UpdateStatusSong(StatusSong newsongStatus, BitmapImage bitmapImage)
        {
            RepeatImage.Source = bitmapImage;
            _statusSong = newsongStatus;
        }

        private void ChangePlayerStatus(PlayerStatus playerStatus)
        {
            _playerStatus = playerStatus;
            if (_playerStatus == PlayerStatus.Play)
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
        RepeatSong = 2,
        Random = 3
    }
    public enum PlayerStatus
    {
        Play = 0,
        Pause = 1
    }
}
