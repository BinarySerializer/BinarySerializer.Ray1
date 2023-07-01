namespace BinarySerializer.Ray1
{
    public class ActiveTable : BinarySerializable
    {
        public short[] ActiveObjects { get; set; }
        public short ActiveObjectsCount { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetRequiredSettings<Ray1Settings>();

            int count;

            if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3)
                count = 49;
            else if (settings.EngineVersionTree.HasParent(Ray1EngineVersion.GBA))
                count = 200;
            else
                count = 100;

            ActiveObjects = s.SerializeArray<short>(ActiveObjects, count, name: nameof(ActiveObjects));
            ActiveObjectsCount = s.Serialize<short>(ActiveObjectsCount, name: nameof(ActiveObjectsCount));
        }
    }
}