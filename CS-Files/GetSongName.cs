namespace AudioPlayer.CS_Files
{
    public class GetSongName
    {
        public static string GetNameOfSong(in int index) 
        {
            string[] songs = SongList.GetSongList();

            //for (int i = 0; i < songs.Length; i++)           
            //    songs[i] = songs[i].Replace('_', ' ');           

            return songs[index];
        }
    }
}
