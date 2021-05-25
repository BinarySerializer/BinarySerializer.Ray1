using System;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Level data for Rayman 2 (PS1 - Demo)
    /// </summary>
    public class R2_LevDataFile : BinarySerializable
    {
        #region Level Data

        public int Uint_00 { get; set; }
        public int Uint_04 { get; set; }

        /// <summary>
        /// Pointer to the objects
        /// </summary>
        public Pointer LoadedObjectsPointer { get; set; }

        /// <summary>
        /// Pointer to the always objects
        /// </summary>
        public Pointer AlwaysObjectsPointer { get; set; }

        /// <summary>
        /// Pointer to the allfix image descriptors
        /// </summary>
        public Pointer FixSpritesPointer { get; set; }
        
        public byte[] Bytes_14 { get; set; }

        public ushort LoadedObjectsCount { get; set; } // NormalObjectsCount + AlwaysObjectSlotsCount
        public ushort AlwaysObjectsCount { get; set; } // These are not in the normal obj array
        public ushort NormalObjectsCount { get; set; } // Normal objects
        public ushort AlwaysObjectSlotsCount { get; set; } // Dummy slots for always objects during runtime

        public ushort UShort_3A { get; set; }

        /// <summary>
        /// The number of allfix image descriptors
        /// </summary>
        public ushort FixSpritesCount { get; set; }

        public ushort UShort_3E { get; set; }
        public ushort UShort_40 { get; set; }
        public ushort UShort_42 { get; set; }
        public ushort UShort_44 { get; set; }
        public ushort UShort_46 { get; set; }
        public uint DevPointer_48 { get; set; } // Dev pointer

        public Pointer ZDCDataPointer { get; set; }
        public Pointer ZDCArray1Pointer { get; set; } // Indexed using ZDCEntry.ZDCIndex
        public Pointer ZDCArray2Pointer { get; set; } // Indexed using ZDCEntry.ZDCIndex and additional value
        public Pointer ZDCArray3Pointer { get; set; } // Indexed using UnkPointer5 values

        /// <summary>
        /// The object link table for all loaded objects
        /// </summary>
        public ushort[] ObjectLinkTable { get; set; }

        #endregion

        #region Parsed Data

        /// <summary>
        /// The objects
        /// </summary>
        public R2_ObjData[] Objects { get; set; }

        /// <summary>
        /// The always objects. These do not have map positions.
        /// </summary>
        public R2_ObjData[] AlwaysObjects { get; set; }

        /// <summary>
        /// The allfix image descriptors
        /// </summary>
        public Sprite[] FixSprites { get; set; }

        public ZDCData[] ZDC { get; set; }
        public ZDC_TriggerFlags[] ZDCTriggerFlags { get; set; }
        public ushort[] ZDCArray2 { get; set; }
        public R2_ZDCUnkData[] ZDCArray3 { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            Uint_00 = s.Serialize<int>(Uint_00, name: nameof(Uint_00));
            Uint_04 = s.Serialize<int>(Uint_04, name: nameof(Uint_04));

            LoadedObjectsPointer = s.SerializePointer(LoadedObjectsPointer, name: nameof(LoadedObjectsPointer));

            AlwaysObjectsPointer = s.SerializePointer(AlwaysObjectsPointer, name: nameof(AlwaysObjectsPointer));
            FixSpritesPointer = s.SerializePointer(FixSpritesPointer, name: nameof(FixSpritesPointer));

            Bytes_14 = s.SerializeArray<byte>(Bytes_14, 30, name: nameof(Bytes_14));

            LoadedObjectsCount = s.Serialize<ushort>(LoadedObjectsCount, name: nameof(LoadedObjectsCount));

            AlwaysObjectsCount = s.Serialize<ushort>(AlwaysObjectsCount, name: nameof(AlwaysObjectsCount));
            NormalObjectsCount = s.Serialize<ushort>(NormalObjectsCount, name: nameof(NormalObjectsCount));
            AlwaysObjectSlotsCount = s.Serialize<ushort>(AlwaysObjectSlotsCount, name: nameof(AlwaysObjectSlotsCount));
            UShort_3A = s.Serialize<ushort>(UShort_3A, name: nameof(UShort_3A));
            FixSpritesCount = s.Serialize<ushort>(FixSpritesCount, name: nameof(FixSpritesCount));
            UShort_3E = s.Serialize<ushort>(UShort_3E, name: nameof(UShort_3E));
            UShort_40 = s.Serialize<ushort>(UShort_40, name: nameof(UShort_40));
            UShort_42 = s.Serialize<ushort>(UShort_42, name: nameof(UShort_42));
            UShort_44 = s.Serialize<ushort>(UShort_44, name: nameof(UShort_44));
            UShort_46 = s.Serialize<ushort>(UShort_46, name: nameof(UShort_46));
            DevPointer_48 = s.Serialize<uint>(DevPointer_48, name: nameof(DevPointer_48));
            
            ZDCDataPointer = s.SerializePointer(ZDCDataPointer, name: nameof(ZDCDataPointer));
            ZDCArray1Pointer = s.SerializePointer(ZDCArray1Pointer, name: nameof(ZDCArray1Pointer));
            ZDCArray2Pointer = s.SerializePointer(ZDCArray2Pointer, name: nameof(ZDCArray2Pointer));
            ZDCArray3Pointer = s.SerializePointer(ZDCArray3Pointer, name: nameof(ZDCArray3Pointer));

            ObjectLinkTable = s.SerializeArray<ushort>(ObjectLinkTable, LoadedObjectsCount, name: nameof(ObjectLinkTable));

            FixSprites = s.DoAt(FixSpritesPointer, () => s.SerializeObjectArray<Sprite>(FixSprites, FixSpritesCount, name: nameof(FixSprites)));

            Objects = s.DoAt(LoadedObjectsPointer, () => s.SerializeObjectArray<R2_ObjData>(Objects, LoadedObjectsCount, name: nameof(Objects)));
            AlwaysObjects = s.DoAt(AlwaysObjectsPointer, () => s.SerializeObjectArray<R2_ObjData>(AlwaysObjects, AlwaysObjectsCount, name: nameof(AlwaysObjects)));

            ZDC = s.DoAt(ZDCDataPointer, () => s.SerializeObjectArray<ZDCData>(ZDC, 237, name: nameof(ZDC)));
            ZDCTriggerFlags = s.DoAt(ZDCArray1Pointer, () => s.SerializeArray<ZDC_TriggerFlags>(ZDCTriggerFlags, 237, name: nameof(ZDCTriggerFlags)));
            ZDCArray2 = s.DoAt(ZDCArray2Pointer, () => s.SerializeArray<ushort>(ZDCArray2, 474, name: nameof(ZDCArray2)));
            ZDCArray3 = s.DoAt(ZDCArray3Pointer, () => s.SerializeObjectArray<R2_ZDCUnkData>(ZDCArray3, 16, name: nameof(ZDCArray3)));
        }

        #endregion

        [Flags]
        public enum ZDC_TriggerFlags : byte
        {
            None = 0,

            Flag_0 = 1 << 0,

            Rayman = 1 << 1,
            Poing_0 = 1 << 2,
            
            Flag_3 = 1 << 3,
            Flag_4 = 1 << 4,
            Flag_5 = 1 << 5,
            Flag_6 = 1 << 6,

            Poing_1 = 1 << 7
        }
    }
}