using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace AudioPlayer.CS_Files
{
    public class AddNewSongs
    {
        public string[] GetFiles()
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".mp3",
                Multiselect = true,
                Filter = "Audio Formats|*.wav;*.flac;*.mp3;*.wma;"
            };

            string[] bufferSongs;
            bool? result = dialog.ShowDialog();

            bufferSongs = dialog.FileNames;
            for (int i = 0; i < bufferSongs.Length; i++)
                if (bufferSongs[i] == "")
                    result = false;

            if (!dialog.CheckPathExists)
            {
                MessageBox.Show("No Path with this name!");
            };

            if (!dialog.CheckFileExists)
            {
                MessageBox.Show("No File with this name!");
            };

            if (result == true && bufferSongs.Length >= 1)
            {
                for (int i = 0; i < dialog.FileNames.Count(); i++)
                    MessageBox.Show(dialog.FileNames[i]);
            }
            else
                MessageBox.Show("No song is choice");

            return bufferSongs;
        }

        public void SaveFiles(string[] files)
        {
            DirectoryInfo musicDirectory = MusicDirectory.GetMusicDirectory();
            FileInfo[] fileInfo = new FileInfo[files.Length];

            for (int j = 0; j < fileInfo.Length; j++)
            {
                fileInfo[j] = new FileInfo(files[j]);
                string musicDirectoryPath = musicDirectory.Name + @"\\" + fileInfo[j].Name;

                try
                {
                    fileInfo[j].CopyTo(musicDirectoryPath, false);
                }
                catch (Exception ex)
                {
                    fileInfo[j].CopyTo(musicDirectoryPath, true);
                    MessageBox.Show(ex.Message);
                }
            }                                                  
        }        
    }
}
