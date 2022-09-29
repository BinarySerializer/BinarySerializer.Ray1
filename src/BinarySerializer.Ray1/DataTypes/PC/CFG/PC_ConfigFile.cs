namespace BinarySerializer.Ray1
{
    /// <summary>
    /// The config data for Rayman 1 on PC
    /// </summary>
    public class PC_ConfigFile : BinarySerializable
    {
        public PC_Language Language { get; set; }

        public uint Port { get; set; }
        public uint Irq { get; set; }
        public uint Dma { get; set; }
        public uint Param { get; set; }
        public uint DeviceID { get; set; }
        public byte NumCard { get; set; }

        // Keys are indexed as 0-3
        public ushort KeyJump { get; set; }
        public ushort KeyWeapon { get; set; }
        public ushort Options_jeu_10 { get; set; }
        public ushort KeyAction { get; set; }

        public ushort MusicCdActive { get; set; } // Options_jeu_12
        public ushort VolumeSound { get; set; } // Options_jeu_13, set as (127 * value / 20), 0-20

        public ushort IsStero { get; set; } // 0 = Mono, 1 = Stereo
        public ushort EDU_VoiceSound { get; set; }
        public bool Mode_Pad { get; set; } // Indicates if the controller setup screen has been shown
        public byte Port_Pad { get; set; }

        public short XPadMax { get; set; }
        public short XPadMin { get; set; }
        public short YPadMax { get; set; }
        public short YPadMin { get; set; }
        public short XPadCentre { get; set; }
        public short YPadCentre { get; set; }

        public byte[] NotBut { get; set; }
        public byte[] Tab_Key { get; set; } // Left, up, right, down, jump, fist, action

        public byte GameModeVideo { get; set; } // pci1 = 0, pci2 = 1
        public byte P486 { get; set; } // pci1 or pci2 = 0, vesa = 1
        public byte SizeScreen { get; set; } // 4/4 = 0, 3/4 = 1, 2/4 = 2, 1/4 = 3
        public PC_Freq Frequence { get; set; }

        public bool FixOn { get; set; } // Scores enabled
        public bool BackgroundOptionOn { get; set; }
        public bool ScrollDiffOn { get; set; }

        public ushort[] RefRam2VramNormalFix { get; set; }
        public ushort[] RefRam2VramNormal { get; set; }
        public ushort[] RefTransFondNormal { get; set; }
        public ushort[] RefSpriteNormal { get; set; }
        public ushort[] RefRam2VramX { get; set; }
        public ushort[] RefVram2VramX { get; set; }
        public ushort[] RefSpriteX { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Get the settings
            var settings = s.GetRequiredSettings<Ray1Settings>();

            if (settings.EngineVersion == Ray1EngineVersion.PC ||
                settings.EngineVersion == Ray1EngineVersion.PocketPC)
                Language = s.Serialize<PC_Language>(Language, name: nameof(Language));

            Port = s.Serialize<uint>(Port, name: nameof(Port));
            Irq = s.Serialize<uint>(Irq, name: nameof(Irq));
            Dma = s.Serialize<uint>(Dma, name: nameof(Dma));
            Param = s.Serialize<uint>(Param, name: nameof(Param));
            DeviceID = s.Serialize<uint>(DeviceID, name: nameof(DeviceID));
            NumCard = s.Serialize<byte>(NumCard, name: nameof(NumCard));
            KeyJump = s.Serialize<ushort>(KeyJump, name: nameof(KeyJump));
            KeyWeapon = s.Serialize<ushort>(KeyWeapon, name: nameof(KeyWeapon));
            Options_jeu_10 = s.Serialize<ushort>(Options_jeu_10, name: nameof(Options_jeu_10));
            KeyAction = s.Serialize<ushort>(KeyAction, name: nameof(KeyAction));

            MusicCdActive = s.Serialize<ushort>(MusicCdActive, name: nameof(MusicCdActive));
            VolumeSound = s.Serialize<ushort>(VolumeSound, name: nameof(VolumeSound));

            IsStero = s.Serialize<ushort>(IsStero, name: nameof(IsStero));

            if (settings.EngineVersion == Ray1EngineVersion.PC_Edu ||
                settings.EngineVersion == Ray1EngineVersion.PC_Kit ||
                settings.EngineVersion == Ray1EngineVersion.PC_Fan)
                EDU_VoiceSound = s.Serialize<ushort>(EDU_VoiceSound, name: nameof(EDU_VoiceSound));

            Mode_Pad = s.Serialize<bool>(Mode_Pad, name: nameof(Mode_Pad));
            Port_Pad = s.Serialize<byte>(Port_Pad, name: nameof(Port_Pad));

            XPadMax = s.Serialize<short>(XPadMax, name: nameof(XPadMax));
            XPadMin = s.Serialize<short>(XPadMin, name: nameof(XPadMin));
            YPadMax = s.Serialize<short>(YPadMax, name: nameof(YPadMax));
            YPadMin = s.Serialize<short>(YPadMin, name: nameof(YPadMin));
            XPadCentre = s.Serialize<short>(XPadCentre, name: nameof(XPadCentre));
            YPadCentre = s.Serialize<short>(YPadCentre, name: nameof(YPadCentre));

            NotBut = s.SerializeArray<byte>(NotBut, 4, name: nameof(NotBut));
            Tab_Key = s.SerializeArray<byte>(Tab_Key, 7, name: nameof(Tab_Key));

            GameModeVideo = s.Serialize<byte>(GameModeVideo, name: nameof(GameModeVideo));
            P486 = s.Serialize<byte>(P486, name: nameof(P486));
            SizeScreen = s.Serialize<byte>(SizeScreen, name: nameof(SizeScreen));
            Frequence = s.Serialize<PC_Freq>(Frequence, name: nameof(Frequence));
            FixOn = s.Serialize<bool>(FixOn, name: nameof(FixOn));
            BackgroundOptionOn = s.Serialize<bool>(BackgroundOptionOn, name: nameof(BackgroundOptionOn));
            ScrollDiffOn = s.Serialize<bool>(ScrollDiffOn, name: nameof(ScrollDiffOn));

            RefRam2VramNormalFix = s.SerializeArray<ushort>(RefRam2VramNormalFix, 8, name: nameof(RefRam2VramNormalFix));
            RefRam2VramNormal = s.SerializeArray<ushort>(RefRam2VramNormal, 8, name: nameof(RefRam2VramNormal));
            RefTransFondNormal = s.SerializeArray<ushort>(RefTransFondNormal, 8, name: nameof(RefTransFondNormal));

            RefSpriteNormal = s.SerializeArray<ushort>(RefSpriteNormal, 2, name: nameof(RefSpriteNormal));
            RefRam2VramX = s.SerializeArray<ushort>(RefRam2VramX, 2, name: nameof(RefRam2VramX));
            RefVram2VramX = s.SerializeArray<ushort>(RefVram2VramX, 2, name: nameof(RefVram2VramX));
            RefSpriteX = s.SerializeArray<ushort>(RefSpriteX, 2, name: nameof(RefSpriteX));
        }
    }
}