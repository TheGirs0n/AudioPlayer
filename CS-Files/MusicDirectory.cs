using System;
using System.IO;
using System.Linq;

namespace AudioPlayer.CS_Files
{
    internal class MusicDirectory
    {
        public static DirectoryInfo GetMusicDirectory()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            DirectoryInfo mainDirectory = new DirectoryInfo(path);

            DirectoryInfo[] subDirectorys = mainDirectory.GetDirectories();

            bool isMusicListExists = subDirectorys.ToList().Exists(dir => dir.Name == "MusicList");

            DirectoryInfo musicDirectory;
            if (isMusicListExists)
            {
                musicDirectory = new DirectoryInfo(subDirectorys.Where(dir => dir.Name == "MusicList").First().Name);
            }
            else
            {
                mainDirectory.CreateSubdirectory("MusicList");
                musicDirectory = new DirectoryInfo(subDirectorys.Where(dir => dir.Name == "MusicList").First().Name);
            }

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
