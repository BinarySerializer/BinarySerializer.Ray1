namespace BinarySerializer.Ray1
{
    public class PC_VersionMemoryInfo : BinarySerializable
    {
        public uint TailleMainMemTmp { get; set; }
        public uint TailleMainMemFix { get; set; }
        public uint TailleMainMemWorld { get; set; }
        public uint TailleMainMemLevel { get; set; }
        public uint TailleMainMemSprite { get; set; }
        public uint TailleMainMemSamplesTable { get; set; }
        public uint TailleMainMemEdit { get; set; }
        public uint TailleMainMemSaveEvent { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            TailleMainMemTmp = s.Serialize<uint>(TailleMainMemTmp, name: nameof(TailleMainMemTmp));
            s.Log("{0}: {1} bytes", nameof(TailleMainMemTmp), TailleMainMemTmp << 10);
            
            TailleMainMemFix = s.Serialize<uint>(TailleMainMemFix, name: nameof(TailleMainMemFix));
            s.Log("{0}: {1} bytes", nameof(TailleMainMemFix), TailleMainMemFix << 10);
            
            TailleMainMemWorld = s.Serialize<uint>(TailleMainMemWorld, name: nameof(TailleMainMemWorld));
            s.Log("{0}: {1} bytes", nameof(TailleMainMemWorld), TailleMainMemWorld << 10);
            
            TailleMainMemLevel = s.Serialize<uint>(TailleMainMemLevel, name: nameof(TailleMainMemLevel));
            s.Log("{0}: {1} bytes", nameof(TailleMainMemLevel), TailleMainMemLevel << 10);
            
            TailleMainMemSprite = s.Serialize<uint>(TailleMainMemSprite, name: nameof(TailleMainMemSprite));
            s.Log("{0}: {1} bytes", nameof(TailleMainMemSprite), TailleMainMemSprite << 10);
            
            TailleMainMemSamplesTable = s.Serialize<uint>(TailleMainMemSamplesTable, name: nameof(TailleMainMemSamplesTable));
            s.Log("{0}: {1} bytes", nameof(TailleMainMemSamplesTable), TailleMainMemSamplesTable << 10);

            if (settings.EngineVersion == Ray1EngineVersion.PC_Kit || settings.EngineVersion == Ray1EngineVersion.PC_Fan)
            {
                TailleMainMemEdit = s.Serialize<uint>(TailleMainMemEdit, name: nameof(TailleMainMemEdit));
                s.Log("{0}: {1} bytes", nameof(TailleMainMemEdit), TailleMainMemEdit << 10);
                
                TailleMainMemSaveEvent = s.Serialize<uint>(TailleMainMemSaveEvent, name: nameof(TailleMainMemSaveEvent));
                s.Log("{0}: {1} bytes", nameof(TailleMainMemSaveEvent), TailleMainMemSaveEvent << 10);
            }
        }
    }
}