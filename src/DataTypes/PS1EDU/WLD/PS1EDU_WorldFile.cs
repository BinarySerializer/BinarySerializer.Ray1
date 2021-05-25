using System.Collections.Generic;
using System.Linq;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// World data for EDU on PS1
    /// </summary>
    public class PS1EDU_WorldFile : BinarySerializable
    {
        #region Public Properties

        // Set this before serializing!
        public Type FileType { get; set; }

        public ushort BG1 { get; set; }

        public ushort BG2 { get; set; }

        public byte Plan0NumPcxCount { get; set; }

        public string[] Plan0NumPcxFiles { get; set; }

        public ushort DESCount { get; set; }
        
        public byte ETACount { get; set; }

        public uint DESBlockLength { get; set; }

        public PS1EDU_DESTemplate[] DESData { get; set; }

        public PC_WorldDefine WorldDefine { get; set; }

        public uint MainDataBlockLength { get; set; }

        public Pointer MainDataBlockPointer { get; set; }

        public Sprite[][] ImageDescriptors { get; set; }

        public PS1EDU_Animation[][] AnimationDescriptors { get; set; }

        /// <summary>
        /// The event states for every ETA
        /// </summary>
        public ObjState[][][] ETA { get; set; }

        // Index table for DES. not sure what for yet
        public uint[] DESDataIndices { get; set; }

        public byte ETAStateCountTableCount { get; set; }

        public byte[] ETAStateCountTable { get; set; }

        public byte ETASubStateCountTableCount { get; set; }

        public byte[] ETASubStateCountTable { get; set; }

        public uint AnimationDescriptorLayersBlockSizeTableCount { get; set; }

        public ushort[] AnimationDescriptorLayersBlockSizeTable { get; set; }

        public AnimationLayer[] AnimationLayers { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            if (FileType == Type.World)
            {
                // Serialize header
                BG1 = s.Serialize<ushort>(BG1, name: nameof(BG1));
                BG2 = s.Serialize<ushort>(BG2, name: nameof(BG2));
                Plan0NumPcxCount = s.Serialize<byte>(Plan0NumPcxCount, name: nameof(Plan0NumPcxCount));
                s.DoXOR(0x19, () => Plan0NumPcxFiles = s.SerializeStringArray(Plan0NumPcxFiles, Plan0NumPcxCount, 8, name: nameof(Plan0NumPcxFiles)));

                // Serialize counts
                DESCount = s.Serialize<ushort>(DESCount, name: nameof(DESCount));
                ETACount = s.Serialize<byte>(ETACount, name: nameof(ETACount));
                DESBlockLength = s.Serialize<uint>(DESBlockLength, name: nameof(DESBlockLength));
            }
            else
            {
                // Serialize header
                ETACount = s.Serialize<byte>(ETACount, name: nameof(ETACount));
                DESCount = s.Serialize<ushort>(DESCount, name: nameof(DESCount));
            }

            // Serialize DES data
            DESData = s.SerializeObjectArray<PS1EDU_DESTemplate>(DESData, DESCount, name: nameof(DESData));

            if (FileType == Type.World)
                WorldDefine = WorldDefine = s.SerializeObject<PC_WorldDefine>(WorldDefine, name: nameof(WorldDefine));

            // Serialize main data block length
            MainDataBlockLength = s.Serialize<uint>(MainDataBlockLength, name: nameof(MainDataBlockLength));

            // We parse the main data block later...
            MainDataBlockPointer = s.CurrentPointer;
            s.Goto(MainDataBlockPointer + MainDataBlockLength);

            if (FileType == Type.Allfix)
                DESDataIndices = s.SerializeArray<uint>(DESDataIndices, 8, name: nameof(DESDataIndices));

            // Serialize ETA tables
            if (FileType == Type.World)
            {
                ETAStateCountTableCount = s.Serialize<byte>(ETAStateCountTableCount, name: nameof(ETAStateCountTableCount));
                ETAStateCountTable = s.SerializeArray<byte>(ETAStateCountTable, ETAStateCountTableCount, name: nameof(ETAStateCountTable));
            }
            else
            {
                ETAStateCountTable = s.SerializeArray<byte>(ETAStateCountTable, ETACount, name: nameof(ETAStateCountTable));
            }
            ETASubStateCountTableCount = s.Serialize<byte>(ETASubStateCountTableCount, name: nameof(ETASubStateCountTableCount));
            ETASubStateCountTable = s.SerializeArray<byte>(ETASubStateCountTable, ETASubStateCountTableCount, name: nameof(ETASubStateCountTable));

            // Serialize animation descriptor layer table
            AnimationDescriptorLayersBlockSizeTableCount = s.Serialize<uint>(AnimationDescriptorLayersBlockSizeTableCount, name: nameof(AnimationDescriptorLayersBlockSizeTableCount));
            AnimationDescriptorLayersBlockSizeTable = s.SerializeArray<ushort>(AnimationDescriptorLayersBlockSizeTable, AnimationDescriptorLayersBlockSizeTableCount, name: nameof(AnimationDescriptorLayersBlockSizeTable));

            // Serialize animation layers
            AnimationLayers = s.SerializeObjectArray<AnimationLayer>(AnimationLayers, 0xFE, name: nameof(AnimationLayers));

            // Serialize the main data block
            s.DoAt(MainDataBlockPointer, () =>
            {
                if (FileType == Type.World)
                {
                    SerializeDES();
                    SerializeETA();
                }
                else
                {
                    SerializeETA();
                    SerializeDES();
                }

                // Helper method for serializing the DES
                void SerializeDES()
                {
                    ImageDescriptors ??= new Sprite[DESCount][];
                    AnimationDescriptors ??= new PS1EDU_Animation[DESCount][];

                    int curAnimDesc = 0;

                    // Serialize data for every DES
                    for (int i = 0; i < DESCount; i++)
                    {
                        // Serialize image descriptors
                        ImageDescriptors[i] = s.SerializeObjectArray<Sprite>(ImageDescriptors[i], DESData[i].SpritesCount, name: $"{nameof(ImageDescriptors)}[{i}]");

                        // Serialize animation descriptors
                        AnimationDescriptors[i] = s.SerializeObjectArray<PS1EDU_Animation>(AnimationDescriptors[i], DESData[i].AnimationsCount, name: $"{nameof(AnimationDescriptors)}[{i}]");

                        // Serialize animation descriptor data
                        for (int j = 0; j < AnimationDescriptors[i].Length; j++)
                        {
                            var descriptor = AnimationDescriptors[i][j];

                            if (descriptor.FrameCount <= 0)
                            {
                                curAnimDesc++;
                                continue;
                            }

                            // Serialize layer data
                            descriptor.LayersData = s.SerializeArray<byte>(descriptor.LayersData, AnimationDescriptorLayersBlockSizeTable[curAnimDesc], name: nameof(descriptor.LayersData));

                            // Padding...
                            if (AnimationDescriptorLayersBlockSizeTable[curAnimDesc] % 4 != 0)
                            {
                                // Padding seems to contain garbage data in this case instead of 0xCD?
                                int paddingLength = 4 - AnimationDescriptorLayersBlockSizeTable[curAnimDesc] % 4;
                                s.SerializeArray<byte>(Enumerable.Repeat((byte)0xCD, paddingLength).ToArray(), paddingLength, name: "Padding");
                            }

                            // Serialize frames
                            if (descriptor.AnimFramesPointer != 0xFFFFFFFF)
                                descriptor.Frames = s.SerializeObjectArray<AnimationFrame>(descriptor.Frames, descriptor.FrameCount, name: nameof(descriptor.Frames));

                            // Parse layers
                            if (descriptor.Layers == null)
                            {
                                var layers = new List<AnimationLayer>();
                                var offset = 0;

                                while (offset < descriptor.LayersData.Length)
                                {
                                    if (descriptor.LayersData[offset] < 2)
                                    {
                                        layers.Add(new AnimationLayer()
                                        {
                                            IsFlippedHorizontally = descriptor.LayersData[offset + 0] == 1,
                                            XPosition = descriptor.LayersData[offset + 1],
                                            YPosition = descriptor.LayersData[offset + 2],
                                            SpriteIndex = descriptor.LayersData[offset + 3],
                                        });

                                        offset += 4;
                                    }
                                    else
                                    {
                                        layers.Add(AnimationLayers[descriptor.LayersData[offset] - 2]);
                                        offset++;
                                    }
                                }

                                descriptor.Layers = layers.ToArray();
                            }

                            curAnimDesc++;
                        }
                    }
                }

                // Helper method for serializing the ETA
                void SerializeETA()
                {
                    if (ETA == null)
                        ETA = new ObjState[ETACount][][];

                    var stateIndex = 0;

                    // Serialize every ETA
                    for (int i = 0; i < ETA.Length; i++)
                    {
                        if (ETA[i] == null)
                            ETA[i] = new ObjState[ETAStateCountTable[i]][];

                        // EDU serializes the pointer structs, but the pointers are invalid. They can be anything as they're overwritten with valid memory pointers upon load
                        uint[] pointerStructs = Enumerable.Repeat((uint)1, ETA[i].Length).ToArray();
                        _ = s.SerializeArray<uint>(pointerStructs, pointerStructs.Length, name: $"ETAPointers[{i}]");

                        // Serialize every state
                        for (int j = 0; j < ETA[i].Length; j++)
                        {
                            // Serialize sub-states
                            ETA[i][j] = s.SerializeObjectArray<ObjState>(ETA[i][j], ETASubStateCountTable[stateIndex], name: $"{nameof(ETA)}[{i}][{j}]");

                            stateIndex++;
                        }
                    }
                }
            });
        }

        #endregion

        #region Enums

        public enum Type
        {
            Allfix,
            World,
        }

        #endregion
    }
}