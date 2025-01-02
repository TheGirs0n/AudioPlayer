using System;
using System.IO;
using System.Text.Json;

namespace AudioPlayer.CS_Files
{
    public class MusicPlayerData
    {
        public int _SongId { get; }
        public string _SongName { get;  }
        public TimeSpan _SongPosition { get;  }
        public MusicPlayerData() { }
        public MusicPlayerData(int songId, string songName, TimeSpan songPosition)
        {
            this._SongId = songId;
            this._SongName = songName;
            this._SongPosition = songPosition;
        }
       
        public void SerializeJSONAsync(MusicPlayerData musicPlayerData)
        {
            File.WriteAllText("MusicData.json", string.Empty);
            using (FileStream fs = new FileStream("MusicData.json", FileMode.OpenOrCreate))
            {
                 JsonSerializer.Serialize(fs, musicPlayerData);
            }
        }
        public MusicPlayerData DeserializeJSON()
        {
            MusicPlayerData musicPlayerData;
            using (FileStream fs = new FileStream("MusicData.json", FileMode.OpenOrCreate))
            {
                musicPlayerData = JsonSerializer.Deserialize<MusicPlayerData>(fs);
            }

            return musicPlayerData;
        }
    }
}
