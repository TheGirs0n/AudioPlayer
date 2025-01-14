using System;
using System.IO;
using System.Text.Json;

namespace AudioPlayer.CS_Files
{
    public class MusicPlayerData
    {
        public int SongId { get; }
        public string SongName { get;  }
        public TimeSpan SongPosition { get;  }
        public MusicPlayerData() { }
        public MusicPlayerData(int songId, string songName, TimeSpan songPosition)
        {
            this.SongId = songId;
            this.SongName = songName;
            this.SongPosition = songPosition;
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
