using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
