namespace BinarySerializer.Ray1
{
    public class PC_GeneralFile : BinarySerializable
    {
        public int WorldMapNumber { get; set; } // Determines the file name of the WLDMAP file

        public PC_CdTracks CdTracks { get; set; }
        public PC_GameInfo GameInfo { get; set; }
        public PC_ChatSamples ChatSamples { get; set; }
        public RecordedDemos RecordedDemos { get; set; }

        public int CreditsCount { get; set; }
        public PC_Credit[] Credits { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            WorldMapNumber = s.Serialize<int>(WorldMapNumber, name: nameof(WorldMapNumber));
            CdTracks = s.SerializeObject<PC_CdTracks>(CdTracks, name: nameof(CdTracks));
            GameInfo = s.SerializeObject<PC_GameInfo>(GameInfo, name: nameof(GameInfo));
            ChatSamples = s.SerializeObject<PC_ChatSamples>(ChatSamples, name: nameof(ChatSamples));
            RecordedDemos = s.SerializeObject<RecordedDemos>(RecordedDemos, name: nameof(RecordedDemos));

            CreditsCount = s.Serialize<int>(CreditsCount, name: nameof(CreditsCount));
            Credits = s.SerializeObjectArray<PC_Credit>(Credits, CreditsCount, name: nameof(Credits));
        }
    }
}