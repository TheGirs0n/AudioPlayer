using AudioPlayer.CS_Files;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Media;

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
        }

        public void InitializeSongs() => songs = audioPlayer.GetSongs(audioPlayer.GetFiles());
        

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            currentsongId = audioPlayer.Play(currentsongId, songs, mediaPlayer);
            // Брать набор музыки из файла
            // Загружать через Load()/LoadAsync()
            // Играть через Play()/PlaySync()
        }

        private void AddNewSong_Click(object sender, RoutedEventArgs e)
        {
            AddNewSongs newSongs = new AddNewSongs();
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
        }

        private void PreviosSong_Click(object sender, RoutedEventArgs e)
        {
            currentsongId = audioPlayer.PlayPrevios(currentsongId, songs, mediaPlayer);
        }
    }
}
