using System;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Object data
    /// </summary>
    public class R2_ObjData : BinarySerializable 
    {
        #region Static Methods

        /// <summary>
        /// Gets a new object instance for Rayman
        /// </summary>
        public static R2_ObjData GetRayman(R2_ObjData rayPos, R2_AllfixFooter data) => new R2_ObjData()
        {
            // Gets loaded at 0x80178DF0 during runtime
            ObjParamsPointer = data.RaymanBehaviorPointer,
            CollisionDataPointer = data.RaymanCollisionDataPointer,
            AnimDataPointer = data.RaymanAnimDataPointer,
            XPosition = (short)(rayPos != null ? (rayPos.XPosition + rayPos.CollisionData.OffsetBX - data.RaymanCollisionData.OffsetBX) : 100),
            YPosition = (short)(rayPos != null ? (rayPos.YPosition + rayPos.CollisionData.OffsetBY - data.RaymanCollisionData.OffsetBY) : 10),
            Etat = 0, // It's supposed to be Etat 2, SubEtat 2, but the ray pos state has a better looking speed
            SubEtat = 19,
            MapLayer = ObjMapLayer.Front,
            Unk2 = new byte[17],
            ObjType = R2_ObjType.Rayman,
            DisplayPrio = 7,
            Bytes_5B = new byte[10],
            Bytes_65 = new byte[3],
            Unk5 = new byte[2],
            CollisionData = data.RaymanCollisionData,
            AnimData = data.RaymanAnimData
        };

        #endregion

        #region Pre-Serialize

        public bool Pre_IsSerializingFromMemory { get; set; }

        #endregion

        #region Obj Data

        public ushort UShort_00 { get; set; }
        public ushort UShort_02 { get; set; }
        public ushort UShort_04 { get; set; }
        public ushort UShort_06 { get; set; }
        public ushort UShort_08 { get; set; }
        public ushort UShort_0A { get; set; }
        
        // 12 (0xC)
        
        public Pointer ObjParamsPointer { get; set; } // The data struct here depends on the object type and acts as additional parameters. Several of these are edited during runtime. The max size is 44 bytes, which all empty always slot objects use.

        // Leads to 16-byte long structures for collision data
        public Pointer CollisionDataPointer { get; set; }

        public Pointer AnimDataPointer { get; set; }

        // Always 0 in file - gets set to the object function struct when initialized, based on the type
        public uint RuntimeHandlersPointer { get; set; }
        
        // 28 (0x1C)

        public short InitialXPosition { get; set; }
        public short InitialYPosition { get; set; }

        // 32 (0x20)

        public byte InitialEtat { get; set; }
        public byte InitialSubEtat { get; set; }
        public byte InitialHitPoints { get; set; }

        // 24 (0x22)

        public byte InitialDisplayPrio { get; set; }

        public ObjMapLayer MapLayer { get; set; }

        // 26 (0x24)

        public byte Byte_25 { get; set; }
        public PS1_R2Demo_ObjFlags Flags { get; set; }
        public byte Byte_27 { get; set; }
        
        public int Float_XPos { get; set; }
        public int Float_YPos { get; set; }

        public byte[] Unk2 { get; set; }

        // 56 (0x38)

        // Dev pointer in file - gets set to a pointer to the current object state during runtime
        public uint RuntimeCurrentStatePointer { get; set; }

        // Always 0 in file - gets set to a pointer during runtime
        public uint RuntimeCurrentAnimationPointer { get; set; }

        // 64 (0x40)

        public ushort Index { get; set; }

        /// <summary>
        /// The object type
        /// </summary>
        public R2_ObjType ObjType { get; set; }

        // 68 (0x44)

        public short XPosition { get; set; }
        public short YPosition { get; set; }

        // 72 (0x48)

        public short ScreenXPosition { get; set; }
        public short ScreenYPosition { get; set; }

        // 76 (0x4C)

        public short Float_SpeedX { get; set; }
        public short Float_SpeedY { get; set; }
        public short Short_50 { get; set; }
        public byte Byte_52 { get; set; }

        public byte RuntimeCurrentAnimIndex { get; set; }

        // 84 (0x54)

        public byte RuntimeCurrentAnimFrame { get; set; }

        public byte Etat { get; set; }
        public byte SubEtat { get; set; }
        public byte HitPoints { get; set; }
        public byte Byte_58 { get; set; }

        // The layer to appear on (0-7)
        public byte DisplayPrio { get; set; }

        // 90 (0x5A)

        public R2_TileCollisionType RuntimeCurrentCollisionType { get; set; }

        public byte[] Bytes_5B { get; set; }

        // 100 (0x64)

        public ObjMapLayer RuntimeMapLayer { get; set; }

        public byte[] Bytes_65 { get; set; }

        public PS1_R2Demo_ObjRuntimeFlags1 RuntimeFlags1 { get; set; }

        // 104 (0x68)

        public bool RuntimeFlipX { get; set; }
        public bool RuntimeUnkFlag { get; set; }
        public byte ZDCFlags { get; set; }

        // First bit determines if the sprite should be faded
        public byte RuntimeFlags3 { get; set; }

        public byte[] Unk5 { get; set; }

        #endregion

        #region Pointer Data

        public R2_ObjCollision CollisionData { get; set; }
        public R2_AnimationData AnimData { get; set; }

        public byte[] ParamsGeneric { get; set; }
        public R2_ObjParams_Gendoor ParamsGendoor { get; set; }
        public R2_ObjParams_Trigger ParamsTrigger { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize initial unknown values
            UShort_00 = s.Serialize<ushort>(UShort_00, name: nameof(UShort_00));
            UShort_02 = s.Serialize<ushort>(UShort_02, name: nameof(UShort_02));
            UShort_04 = s.Serialize<ushort>(UShort_04, name: nameof(UShort_04));
            UShort_06 = s.Serialize<ushort>(UShort_06, name: nameof(UShort_06));
            UShort_08 = s.Serialize<ushort>(UShort_08, name: nameof(UShort_08));
            UShort_0A = s.Serialize<ushort>(UShort_0A, name: nameof(UShort_0A));

            // Serialize pointers
            ObjParamsPointer = s.SerializePointer(ObjParamsPointer, name: nameof(ObjParamsPointer));
            CollisionDataPointer = s.SerializePointer(CollisionDataPointer, name: nameof(CollisionDataPointer));
            AnimDataPointer = s.SerializePointer(AnimDataPointer, name: nameof(AnimDataPointer));
            RuntimeHandlersPointer = s.Serialize<uint>(RuntimeHandlersPointer, name: nameof(RuntimeHandlersPointer));

            // Serialize positions
            InitialXPosition = s.Serialize<short>(InitialXPosition, name: nameof(InitialXPosition));
            InitialYPosition = s.Serialize<short>(InitialYPosition, name: nameof(InitialYPosition));

            // Serialize state data
            InitialEtat = s.Serialize<byte>(InitialEtat, name: nameof(InitialEtat));
            InitialSubEtat = s.Serialize<byte>(InitialSubEtat, name: nameof(InitialSubEtat));
            InitialHitPoints = s.Serialize<byte>(InitialHitPoints, name: nameof(InitialHitPoints));
            InitialDisplayPrio = s.Serialize<byte>(InitialDisplayPrio, name: nameof(InitialDisplayPrio));
            MapLayer = s.Serialize<ObjMapLayer>(MapLayer, name: nameof(MapLayer));

            Byte_25 = s.Serialize<byte>(Byte_25, name: nameof(Byte_25));
            Flags = s.Serialize<PS1_R2Demo_ObjFlags>(Flags, name: nameof(Flags));
            Byte_27 = s.Serialize<byte>(Byte_27, name: nameof(Byte_27));

            Float_XPos = s.Serialize<int>(Float_XPos, name: nameof(Float_XPos));
            Float_YPos = s.Serialize<int>(Float_YPos, name: nameof(Float_YPos));

            Unk2 = s.SerializeArray(Unk2, 8, name: nameof(Unk2)); // Two floats?

            RuntimeCurrentStatePointer = s.Serialize<uint>(RuntimeCurrentStatePointer, name: nameof(RuntimeCurrentStatePointer));
            RuntimeCurrentAnimationPointer = s.Serialize<uint>(RuntimeCurrentAnimationPointer, name: nameof(RuntimeCurrentAnimationPointer));

            Index = s.Serialize<ushort>(Index, name: nameof(Index));

            // Serialize the type
            ObjType = s.Serialize<R2_ObjType>(ObjType, name: nameof(ObjType));

            XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
            YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));

            ScreenXPosition = s.Serialize<short>(ScreenXPosition, name: nameof(ScreenXPosition));
            ScreenYPosition = s.Serialize<short>(ScreenYPosition, name: nameof(ScreenYPosition));

            Float_SpeedX = s.Serialize<short>(Float_SpeedX, name: nameof(Float_SpeedX));
            Float_SpeedY = s.Serialize<short>(Float_SpeedY, name: nameof(Float_SpeedY));
            Short_50 = s.Serialize<short>(Short_50, name: nameof(Short_50));

            Byte_52 = s.Serialize<byte>(Byte_52, name: nameof(Byte_52));

            RuntimeCurrentAnimIndex = s.Serialize<byte>(RuntimeCurrentAnimIndex, name: nameof(RuntimeCurrentAnimIndex));
            RuntimeCurrentAnimFrame = s.Serialize<byte>(RuntimeCurrentAnimFrame, name: nameof(RuntimeCurrentAnimFrame));

            Etat = s.Serialize<byte>(Etat, name: nameof(Etat));
            SubEtat = s.Serialize<byte>(SubEtat, name: nameof(SubEtat));
            HitPoints = s.Serialize<byte>(HitPoints, name: nameof(HitPoints));
            Byte_58 = s.Serialize<byte>(Byte_58, name: nameof(Byte_58));

            DisplayPrio = s.Serialize<byte>(DisplayPrio, name: nameof(DisplayPrio));

            RuntimeCurrentCollisionType = s.Serialize<R2_TileCollisionType>(RuntimeCurrentCollisionType, name: nameof(RuntimeCurrentCollisionType));

            Bytes_5B = s.SerializeArray(Bytes_5B, 9, name: nameof(Bytes_5B));

            RuntimeMapLayer = s.Serialize<ObjMapLayer>(RuntimeMapLayer, name: nameof(RuntimeMapLayer));

            Bytes_65 = s.SerializeArray(Bytes_65, 2, name: nameof(Bytes_65));

            RuntimeFlags1 = s.Serialize<PS1_R2Demo_ObjRuntimeFlags1>(RuntimeFlags1, name: nameof(RuntimeFlags1));

            s.DoBits<byte>(b =>
            {
                RuntimeFlipX = b.SerializeBits<int>(RuntimeFlipX ? 1 : 0, 1, name: nameof(RuntimeFlipX)) == 1;
                RuntimeUnkFlag = b.SerializeBits<int>(RuntimeUnkFlag ? 1 : 0, 1, name: nameof(RuntimeUnkFlag)) == 1;
                ZDCFlags = (byte)b.SerializeBits<int>(ZDCFlags, 6, name: nameof(ZDCFlags));
            });

            RuntimeFlags3 = s.Serialize<byte>(RuntimeFlags3, name: nameof(RuntimeFlags3));

            Unk5 = s.SerializeArray(Unk5, 2, name: nameof(Unk5));

            // Parse data from pointers

            // Serialize collision data
            if (CollisionDataPointer != null)
                s.DoAt(CollisionDataPointer, () => CollisionData = s.SerializeObject<R2_ObjCollision>(CollisionData, name: nameof(CollisionData)));

            // Serialize object params
            s.DoAt(ObjParamsPointer, () =>
            {
                if (ObjType == R2_ObjType.Gendoor || ObjType == R2_ObjType.Killdoor)
                    ParamsGendoor = s.SerializeObject<R2_ObjParams_Gendoor>(ParamsGendoor, name: nameof(ParamsGendoor));
                else if (ObjType == R2_ObjType.Trigger)
                    ParamsTrigger = s.SerializeObject<R2_ObjParams_Trigger>(ParamsTrigger, name: nameof(ParamsTrigger));
                else
                    ParamsGeneric = s.SerializeArray<byte>(ParamsGeneric, 44, name: nameof(ParamsGeneric)); // 44 bytes is the max length for object params
            });

            if (!s.FullSerialize || Pre_IsSerializingFromMemory)
                return;

            // Serialize the animation group data
            if (AnimDataPointer != null)
                s.DoAt(AnimDataPointer, () => AnimData = s.SerializeObject<R2_AnimationData>(AnimData, name: nameof(AnimData)));
        }

        #endregion

        /// <summary>
        /// Flags for <see cref="R2_ObjData"/>
        /// </summary>
        [Flags]
        public enum PS1_R2Demo_ObjFlags : byte
        {
            None = 0,
            UnkFlag_0 = 1 << 0,
            UnkFlag_1 = 1 << 1,
            UnkFlag_2 = 1 << 2,
            UnkFlag_3 = 1 << 3,
            UnkFlag_4 = 1 << 4,
            UnkFlag_5 = 1 << 5,
            UnkFlag_6 = 1 << 6,
            FlippedHorizontally = 1 << 7,
        }

        public enum ObjMapLayer : byte
        {
            None = 0,
            Front = 1,
            Back = 2
        }

        [Flags]
        public enum PS1_R2Demo_ObjRuntimeFlags1 : byte
        {
            None = 0,
            UnkFlag_0 = 1 << 0,
            UnkFlag_1 = 1 << 1,
            UnkFlag_2 = 1 << 2,
            UnkFlag_3 = 1 << 3,

            /// <summary>
            /// Indicates if the object should be drawn on screen
            /// </summary>
            SwitchedOn = 1 << 4,

            UnkFlag_5 = 1 << 5,
            UnkFlag_6 = 1 << 6,
            UnkFlag_7 = 1 << 7,
        }
    }
}