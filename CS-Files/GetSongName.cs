namespace AudioPlayer.CS_Files
{
    public class GetSongName
    {
        public static string GetNameOfSong(in int index) 
        {
            string[] songs = SongList.GetSongList();

            return songs[index];
        }
    }
}
