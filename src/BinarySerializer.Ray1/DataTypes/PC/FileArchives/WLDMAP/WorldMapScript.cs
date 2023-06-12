namespace BinarySerializer.Ray1
{
    public class WorldMapScript : BinarySerializable
    {
        public byte[][] EDU_Alpha { get; set; }
        public WorldMap WorldMap { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            // Serialize alpha data (only on EDU)
            if (settings.EngineVersion == Ray1EngineVersion.PC_Edu ||
                settings.EngineVersion == Ray1EngineVersion.PS1_Edu)
            {
                EDU_Alpha ??= new byte[160][];

                for (int i = 0; i < EDU_Alpha.Length; i++)
                    EDU_Alpha[i] = s.SerializeArray<byte>(EDU_Alpha[i], 256, name: $"{nameof(EDU_Alpha)}[{i}]");
            }

            WorldMap = s.SerializeObject<WorldMap>(WorldMap, name: nameof(WorldMap));
        }
    }
}