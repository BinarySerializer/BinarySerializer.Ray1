namespace BinarySerializer.Ray1
{
    public class PC_WorldFile : PC_BaseWorldFile
    {
        // Used to allocate the buffer
        public ushort MaxBackgroundWidth { get; set; }
        public ushort MaxBackgroundHeight { get; set; }

        // DRM for older versions
        public byte BiosCheckSum { get; set; }
        public byte VideoBiosCheckSum { get; set; }

        /// <summary>
        /// The amount of referenced vignette PCX files
        /// </summary>
        public byte Plan0NumPcxCount { get; set; }

        /// <summary>
        /// The referenced PCX files, indexed (Rayman 1 PC)
        /// </summary>
        public byte[] Plan0NumPcx { get; set; }

        /// <summary>
        /// The referenced PCX files, named (RayKit, EDU etc.)
        /// </summary>
        public string[] Plan0NumPcxFiles { get; set; }

        public PC_WorldDefine WorldDefine { get; set; }

        public string[] DESFileNames { get; set; }

        public string[] ETAFileNames { get; set; }

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetRequiredSettings<Ray1Settings>();

            // Serialize PC Header
            base.SerializeImpl(s);

            // Serialize world data
            MaxBackgroundWidth = s.Serialize<ushort>(MaxBackgroundWidth, name: nameof(MaxBackgroundWidth));
            MaxBackgroundHeight = s.Serialize<ushort>(MaxBackgroundHeight, name: nameof(MaxBackgroundHeight));
            Plan0NumPcxCount = s.Serialize<byte>(Plan0NumPcxCount, name: nameof(Plan0NumPcxCount));
            VideoBiosCheckSum = s.Serialize<byte>(VideoBiosCheckSum, name: nameof(VideoBiosCheckSum));
            BiosCheckSum = s.Serialize<byte>(BiosCheckSum, name: nameof(BiosCheckSum));

            if (settings.EngineVersion == Ray1EngineVersion.PC || settings.EngineVersion == Ray1EngineVersion.PocketPC)
                s.DoProcessed(new Xor8Processor(0x15), () => Plan0NumPcx = s.SerializeArray<byte>(Plan0NumPcx, Plan0NumPcxCount, name: nameof(Plan0NumPcx)));
            else
                s.DoProcessed(new Xor8Processor(0x19), () => Plan0NumPcxFiles = s.SerializeStringArray(Plan0NumPcxFiles, Plan0NumPcxCount, 8, name: nameof(Plan0NumPcxFiles)));

            // Serialize the DES
            DesItemCount = s.Serialize<ushort>(DesItemCount, name: nameof(DesItemCount));

            DesItems = s.SerializeObjectArray<PC_DES>(DesItems, DesItemCount, onPreSerialize: data => data.Pre_FileType = PC_DES.Type.World, name: nameof(DesItems));

            // Serialize the ETA
            Eta = s.SerializeArraySize<PC_ETA, byte>(Eta, name: nameof(Eta));
            Eta = s.SerializeObjectArray<PC_ETA>(Eta, Eta.Length, name: nameof(Eta));

            // Kit and EDU have more data...
            if (settings.EngineVersion == Ray1EngineVersion.PC_Kit || 
                settings.EngineVersion == Ray1EngineVersion.PC_Edu || 
                settings.EngineVersion == Ray1EngineVersion.PC_Fan)
            {
                s.DoProcessed(new Checksum8Processor(), p =>
                {
                    p.Serialize<byte>(s, "WorldDefineChecksum");

                    s.DoProcessed(new Xor8Processor(0x71), () =>
                    {
                        WorldDefine = s.SerializeObject<PC_WorldDefine>(WorldDefine, name: nameof(WorldDefine));
                    });
                });

                // Serialize file tables
                if (settings.EngineVersion == Ray1EngineVersion.PC_Kit || settings.EngineVersion == Ray1EngineVersion.PC_Fan)
                {
                    DESFileNames = s.SerializeStringArray(DESFileNames, 100, 13, name: nameof(DESFileNames));
                    ETAFileNames = s.SerializeStringArray(ETAFileNames, 60, 13, name: nameof(ETAFileNames));
                }
            }
        }
    }
}