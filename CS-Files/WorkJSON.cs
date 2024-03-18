using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AudioPlayer.CS_Files
{
    public class MusicPlayerData
    {
        public int songId { get; set; }
        public string songName { get; set; }
        public TimeSpan songPosition { get; set; }
        public MusicPlayerData() { }
        public MusicPlayerData(int songId, string songName, TimeSpan songPosition)
        {
            this.songId = songId;
            this.songName = songName;
            this.songPosition = songPosition;
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
