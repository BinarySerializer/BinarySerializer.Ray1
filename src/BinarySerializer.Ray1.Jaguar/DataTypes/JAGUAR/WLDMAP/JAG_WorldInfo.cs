namespace BinarySerializer.Ray1.Jaguar
{
    public class JAG_WorldInfo : BinarySerializable
    {
        public short World { get; set; } // Negative for save points
        public short Level { get; set; } // Negative for save points
        public short XPosition { get; set; }
        public short YPosition { get; set; }
        public int Int_08 { get; set; }
        public ushort WorldIndex { get; set; } // The index of the entry in this world
        public byte[] Bytes_0E { get; set; }
        public Pointer<Jag_ColoredString> WorldNamePointer { get; set; }
        public Pointer<Jag_ColoredString> LevelNamePointer { get; set; }
        public Pointer UnusedPointer { get; set; } // Always null
        public JAG_WorldInfoLink[] Links { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            World = s.Serialize<short>(World, name: nameof(World));
            Level = s.Serialize<short>(Level, name: nameof(Level));
            XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
            YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));
            Int_08 = s.Serialize<int>(Int_08, name: nameof(Int_08));
            WorldIndex = s.Serialize<ushort>(WorldIndex, name: nameof(WorldIndex));

            if (settings.EngineVersion == Ray1EngineVersion.Jaguar)
            {
                Bytes_0E = s.SerializeArray<byte>(Bytes_0E, 4, name: nameof(Bytes_0E));
                WorldNamePointer = s.SerializePointer<Jag_ColoredString>(WorldNamePointer, resolve: true, name: nameof(WorldNamePointer));
                LevelNamePointer = s.SerializePointer<Jag_ColoredString>(LevelNamePointer, resolve: true, name: nameof(LevelNamePointer));
                UnusedPointer = s.SerializePointer(UnusedPointer, name: nameof(UnusedPointer));
            }

            Links = s.SerializeObjectArrayUntil<JAG_WorldInfoLink>(Links,
                conditionCheckFunc: x => x.Directions == JAG_WorldInfoLink.Direction.None, 
                getLastObjFunc: () => new JAG_WorldInfoLink(), 
                name: nameof(Links));
        }
    }
}