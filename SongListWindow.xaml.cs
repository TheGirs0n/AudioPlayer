using System.Windows;
using System.Windows.Input;
using AudioPlayer.CS_Files;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для SongListWindow.xaml
    /// </summary>
    public partial class SongListWindow : Window
    { 

        public SongListWindow()
        {
            InitializeComponent();
            LoadSongList();
        }

        void LoadSongList()
        {
            string[] _songs = SongList.GetSongList();
            for(int i = 0; i < _songs.Length; i++)
                SongsList.Items.Add(_songs[i]);
        }

        private void SongsList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SongsList.SelectedItem != null)
            {
                var li = (string)SongsList.Items[SongsList.SelectedIndex];
                MessageBox.Show((string)li);

                
            }
        }
    }
}
