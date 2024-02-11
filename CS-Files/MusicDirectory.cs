using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer.CS_Files
{
    internal class MusicDirectory
    {
        public static DirectoryInfo GetMusicDirectory()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            DirectoryInfo mainDirectory = new DirectoryInfo(path);

            DirectoryInfo[] subDirectorys = mainDirectory.GetDirectories();

            DirectoryInfo musicDirectory = new DirectoryInfo(subDirectorys[0].Name);

            for (int i = 0; i < subDirectorys.Length; i++)
            {
                if (subDirectorys[i].Name == "MusicList")
                {
                    musicDirectory = new DirectoryInfo(subDirectorys[i].FullName);
                                     
                }
            }

            return musicDirectory;
        }

        public static FileInfo[] GetFilesInMusicDirectory()
        {
            var musicDirectory = GetMusicDirectory();
            FileInfo[] musics = musicDirectory.GetFiles();

            return musics;
        }
    }
}
