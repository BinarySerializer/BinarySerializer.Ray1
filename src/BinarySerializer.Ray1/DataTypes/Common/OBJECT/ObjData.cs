using System;
using System.Linq;

namespace BinarySerializer.Ray1
{
    // All offsets in the names are from the PC version

    /// <summary>
    /// An object instance
    /// </summary>
    public class ObjData : BinarySerializable
    {
        #region Static Methods

        /// <summary>
        /// Gets a new obj instance for Rayman
        /// </summary>
        public static ObjData GetRayman(Context context, ObjData rayPos) => CreateObj(context.GetSettings<Ray1Settings>()).InitRayman(context, rayPos);
        public static ObjData GetMapObj(Context context, short x, short y, int index) => CreateObj(context.GetSettings<Ray1Settings>()).InitMapObj(context, x, y, index);
        public static ObjData CreateObj(Ray1Settings settings) => new ObjData
        {
            PS1Demo_Unk1 = new byte[40],
            CommandContexts = new CommandContext[]
            {
                new CommandContext()
            },
            TypeZDC = new ZDCEntry(),
            CollisionTypes = new TileCollisionType[settings.EngineVersion != Ray1EngineVersion.PS1_JPDemoVol3 ? 5 : 1],
        };

        #endregion

        #region Pre-Serialize

        public bool Pre_IsSerializingFromMemory { get; set; }

        #endregion

        #region Header

        // These are indexes in the files and get replaced with pointers during runtime
        public uint PC_SpritesIndex { get; set; }
        public uint PC_AnimationsIndex { get; set; }
        public uint PC_ImageBufferIndex { get; set; }
        public uint PC_ETAIndex { get; set; }

        // Keep separate values for these to avoid invalid pointers when reading from the files
        public uint PC_RuntimeCommandsPointer { get; set; }
        public uint PC_RuntimeLabelOffsetsPointer { get; set; }

        public Pointer SpritesPointer { get; set; }
        public Pointer AnimationsPointer { get; set; }
        public Pointer ImageBufferPointer { get; set; } // Only valid for vol3 PS1 demo and PC
        public Pointer ETAPointer { get; set; }

        public Pointer CommandsPointer { get; set; }
        public Pointer LabelOffsetsPointer { get; set; }

        #endregion

        #region Obj Data

        public byte[] PS1Demo_Unk1 { get; set; }
        public uint PS1_Unk1 { get; set; }

        public CommandContext[] CommandContexts { get; set; }

        // How many of these uints are a part of the CMD context array?
        public uint Uint_1C { get; set; }
        public uint Uint_20 { get; set; }
        public uint IsActive { get; set; } // 0 if inactive, 1 if active - is this a bool or can it be other values too? Game checks if it's 0 to see if always object is inactive.

        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public short PS1Demo_Unk3 { get; set; }

        public uint Uint_30 { get; set; }

        public short Index { get; set; } // This index is used by the game to handle the obj links during runtime

        public short ScreenXPosition { get; set; }
        public short ScreenYPosition { get; set; }
        public short Short_3A { get; set; }

        public short InitialXPosition { get; set; }
        public short InitialYPosition { get; set; }

        public bool PS1Demo_IsFlipped { get; set; }
        public byte PS1Demo_Padding { get; set; }
        public short SpeedX { get; set; }
        public short SpeedY { get; set; }

        public ushort SpritesCount { get; set; }

        public short CurrentCommandOffset { get; set; }
        public short CMD_Arg0 { get; set; } // This along with CMD_Arg1 might be a more generic temp value, so might have other uses too
        public short Short_4A { get; set; } // For Rayman this holds the index of the object he's standing on. It most likely has different uses for other objects based on type. In R2 this is in the type specific data.
        public short Short_4C { get; set; }
        public short Short_4E { get; set; }

        // This value is used for voice lines as a replacement of the normal HitPoints value in order to have a sample index higher than 255. When this is used HitPoints is always EDU_ExtHitPoints % 256.
        public uint EDU_ExtHitPoints { get; set; }
        
        public short CMD_Arg1 { get; set; }
        public short Short_52 { get; set; } // Linked obj index?
        public short Short_54 { get; set; }
        public short Short_56 { get; set; }
        public short Short_58 { get; set; } // Prev collision type for moving platforms
        public short Short_5A { get; set; }

        public ZDCEntry TypeZDC { get; set; }
        public short Short_5E { get; set; }

        public ObjType Type { get; set; }

        public TileCollisionType[] CollisionTypes { get; set; }

        public byte Byte_67 { get; set; }

        public byte OffsetBX { get; set; }
        public byte OffsetBY { get; set; }

        public byte CurrentAnimationIndex { get; set; }
        public byte CurrentAnimationFrame { get; set; }

        public byte SubEtat { get; set; }
        public byte Etat { get; set; }

        public byte InitialSubEtat { get; set; }
        public byte InitialEtat { get; set; }

        public uint CurrentCommand { get; set; }

        public byte OffsetHY { get; set; }

        /// <summary>
        /// The sprite index which uses the obj collision
        /// </summary>
        public byte FollowSprite { get; set; }

        public uint ActualHitPoints
        {
            get => Type == ObjType.EDU_VoiceLine ? EDU_ExtHitPoints : HitPoints;
            set
            {
                if (Type == ObjType.EDU_VoiceLine)
                    EDU_ExtHitPoints = value;

                HitPoints = (byte)(value % 256);
            }
        }

        public byte HitPoints { get; set; }
        public byte InitialHitPoints { get; set; }

        // Appears to be unused. Gets set to 7 for some objects in code, but never appears to be used. Is also set from one of the .mlt values. Perhaps the display prio to be used in the editor?
        public byte UnusedDisplayPrio { get; set; }

        public byte HitSprite { get; set; }

        public byte PS1_Unk5 { get; set; }

        public byte Byte_7A { get; set; }
        public byte Byte_7B { get; set; }
        public byte CurrentCommandContext { get; set; }
        public byte Byte_7D { get; set; }
        public byte PS1Demo_Unk5 { get; set; }
        public byte PS1Demo_Unk6 { get; set; }
        public byte PS1Demo_Unk7 { get; set; }
        public byte PS1Demo_Unk8 { get; set; }

        /// <summary>
        /// The layer the obj sprites gets drawn to, between 1 and 7
        /// </summary>
        public byte DisplayPrio { get; set; }

        public byte Byte_7F { get; set; }

        public byte AnimationsCount { get; set; }

        public PC_ObjFlags PC_Flags { get; set; }

        public PS1_ObjFlags PS1_RuntimeFlags { get; set; }
        public byte PS1_Flags { get; set; }
        public byte PS1_Unk7 { get; set; }

        public ushort Ushort_82 { get; set; }

        #endregion

        #region Helper Data

        public bool IsPCFormat(Ray1Settings settings) => settings.EngineBranch == Ray1EngineBranch.PC || 
                                                         settings.EngineBranch == Ray1EngineBranch.GBA;

        public bool GetFollowEnabled(Ray1Settings settings)
        {
            if (IsPCFormat(settings))
            {
                return PC_Flags.HasFlag(PC_ObjFlags.FollowEnabled);
            }
            else
            {
                var offset = settings.EngineVersion == Ray1EngineVersion.Saturn ? 7 : 0;

                return BitHelpers.ExtractBits(PS1_Flags, 1, offset) == 1;
            }
        }

        public void SetFollowEnabled(Ray1Settings settings, bool value)
        {
            if (IsPCFormat(settings))
            {
                if (value)
                    PC_Flags |= PC_ObjFlags.FollowEnabled;
                else
                    PC_Flags &= ~PC_ObjFlags.FollowEnabled;
            }
            else
            {
                var offset = settings.EngineVersion == Ray1EngineVersion.Saturn ? 7 : 0;

                PS1_Flags = (byte)BitHelpers.SetBits(PS1_Flags, value ? 1 : 0, 1, offset);
            }
        }

        #endregion

        #region Parsed From Pointers

        /// <summary>
        /// The sprites
        /// </summary>
        public SpriteCollection SpriteCollection { get; set; }

        /// <summary>
        /// The animations
        /// </summary>
        public AnimationCollection AnimationCollection { get; set; }

        /// <summary>
        /// Image buffer
        /// </summary>
        public byte[] ImageBuffer { get; set; }

        /// <summary>
        /// The obj commands
        /// </summary>
        public CommandCollection Commands { get; set; }

        /// <summary>
        /// The command label offsets
        /// </summary>
        public ushort[] LabelOffsets { get; set; }

        /// <summary>
        /// The ETA
        /// </summary>
        public ETA ETA { get; set; }

        #endregion

        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            if (!IsPCFormat(settings) || Pre_IsSerializingFromMemory || settings.EngineVersion == Ray1EngineVersion.GBA || settings.EngineVersion == Ray1EngineVersion.DSi)
            {
                SpritesPointer = s.SerializePointer(SpritesPointer, name: nameof(SpritesPointer));
                AnimationsPointer = s.SerializePointer(AnimationsPointer, name: nameof(AnimationsPointer));
                ImageBufferPointer = s.SerializePointer(ImageBufferPointer, allowInvalid: true, name: nameof(ImageBufferPointer));
                ETAPointer = s.SerializePointer(ETAPointer, name: nameof(ETAPointer));

                CommandsPointer = s.SerializePointer(CommandsPointer, name: nameof(CommandsPointer));

                if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3 || settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6)
                {
                    PS1Demo_Unk1 = s.SerializeArray<byte>(PS1Demo_Unk1, 40, name: nameof(PS1Demo_Unk1));

                    Index = s.Serialize<short>(Index, name: nameof(Index));
                    ScreenXPosition = s.Serialize<short>(ScreenXPosition, name: nameof(ScreenXPosition));
                    ScreenYPosition = s.Serialize<short>(ScreenYPosition, name: nameof(ScreenYPosition));
                }
                else
                {
                    LabelOffsetsPointer = s.SerializePointer(LabelOffsetsPointer, name: nameof(LabelOffsetsPointer));

                    if (!IsPCFormat(settings))
                        PS1_Unk1 = s.Serialize<uint>(PS1_Unk1, name: nameof(PS1_Unk1));
                }
            }
            else
            {
                PC_SpritesIndex = s.Serialize<uint>(PC_SpritesIndex, name: nameof(PC_SpritesIndex));
                PC_AnimationsIndex = s.Serialize<uint>(PC_AnimationsIndex, name: nameof(PC_AnimationsIndex));
                PC_ImageBufferIndex = s.Serialize<uint>(PC_ImageBufferIndex, name: nameof(PC_ImageBufferIndex));
                PC_ETAIndex = s.Serialize<uint>(PC_ETAIndex, name: nameof(PC_ETAIndex));

                PC_RuntimeCommandsPointer = s.Serialize<uint>(PC_RuntimeCommandsPointer, name: nameof(PC_RuntimeCommandsPointer));
                PC_RuntimeLabelOffsetsPointer = s.Serialize<uint>(PC_RuntimeLabelOffsetsPointer, name: nameof(PC_RuntimeLabelOffsetsPointer));
            }

            if (IsPCFormat(settings))
            {
                CommandContexts = s.SerializeObjectArray<CommandContext>(CommandContexts, 1, name: nameof(CommandContexts));
                Uint_1C = s.Serialize<uint>(Uint_1C, name: nameof(Uint_1C));
                Uint_20 = s.Serialize<uint>(Uint_20, name: nameof(Uint_20));
                IsActive = s.Serialize<uint>(IsActive, name: nameof(IsActive));
            }

            if (IsPCFormat(settings))
            {
                XPosition = s.Serialize<int>(XPosition, name: nameof(XPosition));
                YPosition = s.Serialize<int>(YPosition, name: nameof(YPosition));
            }
            else
            {
                XPosition = s.Serialize<short>((short)XPosition, name: nameof(XPosition));
                YPosition = s.Serialize<short>((short)YPosition, name: nameof(YPosition));
            }

            if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3 || settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6)
            {
                PS1Demo_Unk3 = s.Serialize<short>(PS1Demo_Unk3, name: nameof(PS1Demo_Unk3));
            }
            else
            {
                if (IsPCFormat(settings))
                    Uint_30 = s.Serialize<uint>(Uint_30, name: nameof(Uint_30));

                Index = s.Serialize<short>(Index, name: nameof(Index));
                ScreenXPosition = s.Serialize<short>(ScreenXPosition, name: nameof(ScreenXPosition));
                ScreenYPosition = s.Serialize<short>(ScreenYPosition, name: nameof(ScreenYPosition));
                Short_3A = s.Serialize<short>(Short_3A, name: nameof(Short_3A));
            }

            InitialXPosition = s.Serialize<short>(InitialXPosition, name: nameof(InitialXPosition));
            InitialYPosition = s.Serialize<short>(InitialYPosition, name: nameof(InitialYPosition));

            if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3)
            {
                PS1Demo_IsFlipped = s.Serialize<bool>(PS1Demo_IsFlipped, name: nameof(PS1Demo_IsFlipped)); // This is stored as a short in the game, but used as a bool
                PS1Demo_Padding = s.Serialize<byte>(PS1Demo_Padding, name: nameof(PS1Demo_Padding));
            }

            SpeedX = s.Serialize<short>(SpeedX, name: nameof(SpeedX));
            SpeedY = s.Serialize<short>(SpeedY, name: nameof(SpeedY));

            SpritesCount = s.Serialize<ushort>(SpritesCount, name: nameof(SpritesCount));

            CurrentCommandOffset = s.Serialize<short>(CurrentCommandOffset, name: nameof(CurrentCommandOffset));
            CMD_Arg0 = s.Serialize<short>(CMD_Arg0, name: nameof(CMD_Arg0));

            Short_4A = s.Serialize<short>(Short_4A, name: nameof(Short_4A));
            Short_4C = s.Serialize<short>(Short_4C, name: nameof(Short_4C));
            Short_4E = s.Serialize<short>(Short_4E, name: nameof(Short_4E));

            if (settings.EngineVersion == Ray1EngineVersion.PC_Kit || 
                settings.EngineVersion == Ray1EngineVersion.PC_Fan ||
                settings.EngineVersion == Ray1EngineVersion.PC_Edu ||
                settings.EngineVersion == Ray1EngineVersion.PS1_Edu)
                EDU_ExtHitPoints = s.Serialize<uint>(EDU_ExtHitPoints, name: nameof(EDU_ExtHitPoints));

            CMD_Arg1 = s.Serialize<short>(CMD_Arg1, name: nameof(CMD_Arg1));
            Short_52 = s.Serialize<short>(Short_52, name: nameof(Short_52));
            Short_54 = s.Serialize<short>(Short_54, name: nameof(Short_54));
            Short_56 = s.Serialize<short>(Short_56, name: nameof(Short_56));
            
            Short_58 = s.Serialize<short>(Short_58, name: nameof(Short_58));
            Short_5A = s.Serialize<short>(Short_5A, name: nameof(Short_5A));
            TypeZDC = s.SerializeObject<ZDCEntry>(TypeZDC, name: nameof(TypeZDC));
            Short_5E = s.Serialize<short>(Short_5E, name: nameof(Short_5E));

            if (IsPCFormat(settings))
                Type = s.Serialize<ObjType>(Type, name: nameof(Type));

            CollisionTypes = s.SerializeArray<TileCollisionType>(CollisionTypes, settings.EngineVersion != Ray1EngineVersion.PS1_JPDemoVol3 ? 5 : 1, name: nameof(CollisionTypes));
            Byte_67 = s.Serialize<byte>(Byte_67, name: nameof(Byte_67));

            OffsetBX = s.Serialize<byte>(OffsetBX, name: nameof(OffsetBX));
            OffsetBY = s.Serialize<byte>(OffsetBY, name: nameof(OffsetBY));

            CurrentAnimationIndex = s.Serialize<byte>(CurrentAnimationIndex, name: nameof(CurrentAnimationIndex));
            CurrentAnimationFrame = s.Serialize<byte>(CurrentAnimationFrame, name: nameof(CurrentAnimationFrame));

            if (IsPCFormat(settings))
            {
                SubEtat = s.Serialize<byte>(SubEtat, name: nameof(SubEtat));
                Etat = s.Serialize<byte>(Etat, name: nameof(Etat));

                InitialSubEtat = s.Serialize<byte>(InitialSubEtat, name: nameof(InitialSubEtat));
                InitialEtat = s.Serialize<byte>(InitialEtat, name: nameof(InitialEtat));
            }
            else
            {
                Etat = s.Serialize<byte>(Etat, name: nameof(Etat));
                InitialEtat = s.Serialize<byte>(InitialEtat, name: nameof(InitialEtat));
                SubEtat = s.Serialize<byte>(SubEtat, name: nameof(SubEtat));
                InitialSubEtat = s.Serialize<byte>(InitialSubEtat, name: nameof(InitialSubEtat));
            }

            CurrentCommand = s.Serialize<uint>(CurrentCommand, name: nameof(CurrentCommand));

            OffsetHY = s.Serialize<byte>(OffsetHY, name: nameof(OffsetHY));

            if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3 || settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6)
                PS1_Flags = s.Serialize<byte>(PS1_Flags, name: nameof(PS1_Flags));

            FollowSprite = s.Serialize<byte>(FollowSprite, name: nameof(FollowSprite));
            HitPoints = s.Serialize<byte>(HitPoints, name: nameof(HitPoints));
            InitialHitPoints = s.Serialize<byte>(InitialHitPoints, name: nameof(InitialHitPoints));
            UnusedDisplayPrio = s.Serialize<byte>(UnusedDisplayPrio, name: nameof(UnusedDisplayPrio));

            if (!IsPCFormat(settings))
                Type = (ObjType)s.Serialize<byte>((byte)Type, name: nameof(Type));

            HitSprite = s.Serialize<byte>(HitSprite, name: nameof(HitSprite));

            if (!IsPCFormat(settings))
                PS1_Unk5 = s.Serialize<byte>(PS1_Unk5, name: nameof(PS1_Unk5));

            Byte_7A = s.Serialize<byte>(Byte_7A, name: nameof(Byte_7A));
            Byte_7B = s.Serialize<byte>(Byte_7B, name: nameof(Byte_7B));
            CurrentCommandContext = s.Serialize<byte>(CurrentCommandContext, name: nameof(CurrentCommandContext));
            Byte_7D = s.Serialize<byte>(Byte_7D, name: nameof(Byte_7D));

            if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3 || settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6)
            {
                PS1Demo_Unk5 = s.Serialize<byte>(PS1Demo_Unk5, name: nameof(PS1Demo_Unk5));

                if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3)
                {
                    PS1Demo_Unk6 = s.Serialize<byte>(PS1Demo_Unk6, name: nameof(PS1Demo_Unk6));
                    PS1Demo_Unk7 = s.Serialize<byte>(PS1Demo_Unk7, name: nameof(PS1Demo_Unk7));
                    PS1Demo_Unk8 = s.Serialize<byte>(PS1Demo_Unk8, name: nameof(PS1Demo_Unk8));
                }
            }

            DisplayPrio = s.Serialize<byte>(DisplayPrio, name: nameof(DisplayPrio));
            Byte_7F = s.Serialize<byte>(Byte_7F, name: nameof(Byte_7F));

            AnimationsCount = s.Serialize<byte>(AnimationsCount, name: nameof(AnimationsCount));

            if (IsPCFormat(settings))
            {
                PC_Flags = s.Serialize<PC_ObjFlags>(PC_Flags, name: nameof(PC_Flags));
                Ushort_82 = s.Serialize<ushort>(Ushort_82, name: nameof(Ushort_82));
            }
            else
            {
                if (settings.EngineVersion != Ray1EngineVersion.PS1_JPDemoVol3)
                {
                    if (settings.EngineVersion != Ray1EngineVersion.PS1_JPDemoVol6)
                    {
                        PS1_RuntimeFlags = s.Serialize<PS1_ObjFlags>(PS1_RuntimeFlags, name: nameof(PS1_RuntimeFlags));
                        PS1_Flags = s.Serialize<byte>(PS1_Flags, name: nameof(PS1_Flags));
                    }

                    PS1_Unk7 = s.Serialize<byte>(PS1_Unk7, name: nameof(PS1_Unk7));
                }
            }

            // Parse data from pointers only on PS1 and if we're not reading from processed memory
            if (IsPCFormat(settings) || Pre_IsSerializingFromMemory || !s.FullSerialize) 
                return;

            // Serialize the sprites
            s.DoAt(SpritesPointer, () => SpriteCollection = s.SerializeObject<SpriteCollection>(SpriteCollection, x => x.Pre_SpritesCount = SpritesCount, name: nameof(SpriteCollection)));

            // Serialize the animations
            s.DoAt(AnimationsPointer, () => AnimationCollection = s.SerializeObject<AnimationCollection>(AnimationCollection, x => x.Pre_AnimationsCount = AnimationsCount, name: nameof(AnimationCollection)));

            if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3)
            {
                if (ImageBuffer == null && ImageBufferPointer != null && SpriteCollection != null)
                {
                    // Determine length of image buffer
                    uint length = 0;
                    foreach (Sprite img in SpriteCollection.Sprites)
                    {
                        if (img.ImageType != 2 && img.ImageType != 3)
                            continue;

                        uint curLength = img.ImageBufferOffset;

                        if (img.ImageType == 2)
                            curLength += (uint)(img.Width / 2) * img.Height;
                        else if (img.ImageType == 3)
                            curLength += (uint)img.Width * img.Height;

                        if (curLength > length)
                            length = curLength;
                    }
                    ImageBuffer = new byte[length];
                }
                s.DoAt(ImageBufferPointer, () => ImageBuffer = s.SerializeArray<byte>(ImageBuffer, ImageBuffer.Length, name: nameof(ImageBuffer)));
            }

            // Serialize the commands
            if (CommandsPointer != null)
                s.DoAt(CommandsPointer, () => Commands = s.SerializeObject<CommandCollection>(Commands, name: nameof(Commands)));

            // Serialize the label offsets
            if (LabelOffsetsPointer != null && Commands != null && Commands.Commands.Length > 0)
            {
                s.DoAt(LabelOffsetsPointer, () =>
                {
                    if (LabelOffsets == null) {
                        int length = Commands.Commands.Max(c => c.UsesLabelOffsets ? (int)c.Arguments[0] : -1) + 1;

                        LabelOffsets = new ushort[length];
                    }
                    // Serialize the label offsets
                    LabelOffsets = s.SerializeArray(LabelOffsets, LabelOffsets.Length, name: nameof(LabelOffsets));
                });
            }

            // Serialize ETA
            if (ETAPointer != null)
                s.DoAt(ETAPointer, () => ETA = s.SerializeObject<ETA>(ETA, name: nameof(ETA)));

            if (ETA?.States?.ElementAtOrDefault(Etat)?.ElementAtOrDefault(SubEtat) == null)
                s.LogWarning($"Matching obj state not found for obj {Type} at {XPosition}x{YPosition} with E{Etat},SE{SubEtat} for {settings.EngineVersion} in {settings.World}{settings.Level}");
        }

        public ObjData InitRayman(Context context, ObjData rayPos)
        {
            var settings = context.GetSettings<Ray1Settings>();

            OffsetBX = 80;
            OffsetBY = (byte)(settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3 || 
                              settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6 ? 80 : 78);
            if (rayPos != null)
            {
                XPosition = rayPos.XPosition + rayPos.OffsetBX - OffsetBX;
                YPosition = rayPos.YPosition + rayPos.OffsetBY - OffsetBY;
            }
            else
            {
                XPosition = 100;
                YPosition = 10;
            }
            Type = ObjType.TYPE_RAYMAN;
            SubEtat = 2;
            Etat = 2;
            OffsetHY = 20;
            FollowSprite = 0;
            HitPoints = 0;
            DisplayPrio = UnusedDisplayPrio = 7;
            HitSprite = 0;

            PC_SpritesIndex = 1;
            PC_AnimationsIndex = 1;
            PC_ImageBufferIndex = 1;
            PC_ETAIndex = 0;

            CommandContexts = new CommandContext[]
            {
                new CommandContext()
            };
            CollisionTypes = new TileCollisionType[5];

            Commands = new CommandCollection()
            {
                Commands = new Command[0]
            };
            LabelOffsets = new ushort[0];

            return this;
        }

        // Copied from INIT_CHEMIN
        public ObjData InitMapObj(Context context, short x, short y, int index)
        {
            var settings = context.GetSettings<Ray1Settings>();

            Type = ObjType.TYPE_MEDAILLON;
            Etat = 5;

            // Set correct sub-etat and position
            if (settings.EngineVersion == Ray1EngineVersion.PC_Kit || settings.EngineVersion == Ray1EngineVersion.PC_Fan)
            {
                SubEtat = 69;

                // ?
                XPosition = x - 34;
                YPosition = y - 39;

                // ?
                //OffsetBX = 80;
                //OffsetBY = 64;
            }
            else if (settings.EngineVersion == Ray1EngineVersion.PC_Edu || settings.EngineVersion == Ray1EngineVersion.PS1_Edu)
            {
                if (index == 0) // Normal
                    SubEtat = 39;
                else if (index == 2) // End
                    SubEtat = 55;
                else if (index == 4) // Demo
                    SubEtat = 54;
                else if (index == 3) // Start point
                    SubEtat = 45;

                // ?
                XPosition = x - 34 - 36;
                YPosition = y - 39 - 25;

                // ?
                //OffsetBX = 80;
                //OffsetBY = 64;
            }
            else
            {
                XPosition = x - 70; // In the code it's 78 - why do we have to offset it differently here?
                YPosition = y - 64;

                OffsetBX = 80;
                OffsetBY = 64;

                // Mr Dark
                if (index == 17)
                    SubEtat = 59;
                else if (index > 17)
                    SubEtat = 58;
                else
                    SubEtat = 39;
            }

            PC_SpritesIndex = 4;
            PC_AnimationsIndex = 4;
            PC_ImageBufferIndex = 4;
            PC_ETAIndex = 2;

            CommandContexts = new CommandContext[]
            {
                new CommandContext()
            };
            CollisionTypes = new TileCollisionType[5];

            Commands = new CommandCollection()
            {
                Commands = new Command[0]
            };
            LabelOffsets = new ushort[0];

            return this;
        }

        /// <summary>
        /// Flags for an object on PC. All values are runtime only except for FollowEnabled.
        /// </summary>
        [Flags]
        public enum PC_ObjFlags : byte
        {
            None = 0,

            Flag_0 = 1 << 0,

            /// <summary>
            /// A flag used for commands
            /// </summary>
            Test = 1 << 1,

            /// <summary>
            /// Indicates if the object should be drawn on screen
            /// </summary>
            SwitchedOn = 1 << 2,

            /// <summary>
            /// Indicates if the object should be flipped
            /// </summary>
            IsFlipped = 1 << 3,

            Flag_4 = 1 << 4,

            /// <summary>
            /// Indicates if the object has collision
            /// </summary>
            FollowEnabled = 1 << 5,

            Flag_6 = 1 << 6,

            // Appears related to the displaying animation. Changes a lot when an animation is playing.
            Flag_7 = 1 << 7,
        }

        [Flags]
        public enum PS1_ObjFlags : byte
        {
            None = 0,

            UnkFlag_0 = 1 << 0,
            UnkFlag_1 = 1 << 1,
            UnkFlag_2 = 1 << 2,
            SwitchedOn = 1 << 3,
            UnkFlag_4 = 1 << 4,
            UnkFlag_5 = 1 << 5,
            IsFlipped = 1 << 6,
            UnkFlag_7 = 1 << 7,
        }

        public class CommandContext : BinarySerializable
        {
            /// <summary>
            /// The offset where the context was stored, used to remember where to jump back to after execution of the sub-function has finished
            /// </summary>
            public ushort CmdOffset { get; set; }

            /// <summary>
            /// The amount of times the execution should repeat before continuing, used for loops
            /// </summary>
            public ushort Count { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                CmdOffset = s.Serialize<ushort>(CmdOffset, name: nameof(CmdOffset));
                Count = s.Serialize<ushort>(Count, name: nameof(Count));
            }
        }
    }
}