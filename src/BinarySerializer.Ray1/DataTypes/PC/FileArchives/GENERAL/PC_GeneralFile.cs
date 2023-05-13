namespace BinarySerializer.Ray1
{
    public class PC_GeneralFile : BinarySerializable
    {
        public int WorldMapNumber { get; set; } // Determines the file name of the WLDMAP file

        public PC_CdTracks CdTracks { get; set; }
        public PC_GameInfo GameInfo { get; set; }
        public PC_ChatSamples ChatSamples { get; set; }

        public byte DemosCount { get; set; }
        public int MaxDemoLength { get; set; }
        public int Uint_0C9D { get; set; }
        public PC_RecordedDemo[] Demos { get; set; }

        public int CreditsCount { get; set; }
        public PC_Credit[] Credits { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            WorldMapNumber = s.Serialize<int>(WorldMapNumber, name: nameof(WorldMapNumber));
            CdTracks = s.SerializeObject<PC_CdTracks>(CdTracks, name: nameof(CdTracks));
            GameInfo = s.SerializeObject<PC_GameInfo>(GameInfo, name: nameof(GameInfo));
            ChatSamples = s.SerializeObject<PC_ChatSamples>(ChatSamples, name: nameof(ChatSamples));

            DemosCount = s.Serialize<byte>(DemosCount, name: nameof(DemosCount));
            MaxDemoLength = s.Serialize<int>(MaxDemoLength, name: nameof(MaxDemoLength));
            Uint_0C9D = s.Serialize<int>(Uint_0C9D, name: nameof(Uint_0C9D));

            Demos = s.SerializeObjectArray<PC_RecordedDemo>(Demos, DemosCount, name: nameof(Demos));

            CreditsCount = s.Serialize<int>(CreditsCount, name: nameof(CreditsCount));
            Credits = s.SerializeObjectArray<PC_Credit>(Credits, CreditsCount, name: nameof(Credits));
        }
    }
}