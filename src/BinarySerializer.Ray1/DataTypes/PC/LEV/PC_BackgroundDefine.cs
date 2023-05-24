namespace BinarySerializer.Ray1
{
    public class PC_BackgroundDefine : BinarySerializable
    {
        public BackgroundSpritePosition[] SpritePositions { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();
            bool isEncryptedAndChecksum = settings.EngineVersion != Ray1EngineVersion.PS1_Edu;

            s.DoProcessed(isEncryptedAndChecksum ? new Checksum8Processor() : null, p =>
            {
                p?.Serialize<byte>(s, "BackgroundDefineChecksum");

                s.DoProcessed(isEncryptedAndChecksum ? new Xor8Processor(0xA5) : null, () =>
                {
                    SpritePositions = s.SerializeObjectArray<BackgroundSpritePosition>(SpritePositions, 6, name: nameof(SpritePositions));
                });
            });
        }
    }
}