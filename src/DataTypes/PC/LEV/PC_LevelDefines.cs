using System;

namespace BinarySerializer.Ray1
{
    public class PC_LevelDefines : BinarySerializable
    {
        public byte LevelDefineChecksum { get; set; }
        public uint CDTrackAdress { get; set; } // Gets filled in during runtime
        public byte[] LevelDefine_4 { get; set; }
        public byte CDTrack { get; set; }

        public byte CurrentFNDIndex { get; set; } // Gets set during runtime
        public byte FNDIndex { get; set; }
        public byte ScrollDiffFNDIndex { get; set; }

        public LevelEffectFlags EffectFlags { get; set; }
        
        public RayEvts RayEvts { get; set; }
        public byte UnkByte { get; set; } // Padding?

        public byte BackgroundDefineNormalChecksum { get; set; }
        public BackgroundLayerPosition[] BackgroundDefineNormal { get; set; }

        public byte BackgroundDefineDiffChecksum { get; set; }
        public BackgroundLayerPosition[] BackgroundDefineDiff { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            var isEncryptedAndChecksum = settings.EngineVersion != Ray1EngineVersion.PS1_Edu;

            LevelDefineChecksum = s.DoChecksum(new Checksum8Calculator(false), () =>
            {
                s.DoXOR((byte)(isEncryptedAndChecksum ? 0x57 : 0), () =>
                {
                    CDTrackAdress = s.Serialize<uint>(CDTrackAdress, name: nameof(CDTrackAdress));
                    LevelDefine_4 = s.SerializeArray<byte>(LevelDefine_4, 2, name: nameof(LevelDefine_4));
                    CDTrack = s.Serialize<byte>(CDTrack, name: nameof(CDTrack));
                    CurrentFNDIndex = s.Serialize<byte>(CurrentFNDIndex, name: nameof(CurrentFNDIndex));
                    FNDIndex = s.Serialize<byte>(FNDIndex, name: nameof(FNDIndex));
                    ScrollDiffFNDIndex = s.Serialize<byte>(ScrollDiffFNDIndex, name: nameof(ScrollDiffFNDIndex));
                    s.SerializePadding(1);
                    EffectFlags = s.Serialize<LevelEffectFlags>(EffectFlags, name: nameof(EffectFlags));
                    RayEvts = s.Serialize<RayEvts>(RayEvts, name: nameof(RayEvts));
                    UnkByte = s.Serialize<byte>(UnkByte, name: nameof(UnkByte));
                });
            }, ChecksumPlacement.Before, calculateChecksum: isEncryptedAndChecksum, name: nameof(LevelDefineChecksum));

            BackgroundDefineNormalChecksum = s.DoChecksum(new Checksum8Calculator(false), () =>
            {
                s.DoXOR((byte)(isEncryptedAndChecksum ? 0xA5 : 0), () => BackgroundDefineNormal = s.SerializeObjectArray<BackgroundLayerPosition>(BackgroundDefineNormal, 6, name: nameof(BackgroundDefineNormal)));
            }, ChecksumPlacement.Before, calculateChecksum: isEncryptedAndChecksum, name: nameof(BackgroundDefineNormalChecksum));

            BackgroundDefineDiffChecksum = s.DoChecksum(new Checksum8Calculator(false), () =>
            {
                s.DoXOR((byte)(isEncryptedAndChecksum ? 0xA5 : 0), () => BackgroundDefineDiff = s.SerializeObjectArray<BackgroundLayerPosition>(BackgroundDefineDiff, 6, name: nameof(BackgroundDefineDiff)));
            }, ChecksumPlacement.Before, calculateChecksum: isEncryptedAndChecksum, name: nameof(BackgroundDefineDiffChecksum));
        }

        [Flags]
        public enum LevelEffectFlags : ushort
        {
            // Invalid combos:
            // Storm + firefly (firefly is specified in RayEvts)
            // Rain + snow
            // Wind without rain
            // Hot effect with differential scroll

            None = 0,

            Effect_0 = 1 << 0,
            Effect_1 = 1 << 1,

            LockHorizontalCamera = 1 << 2,
            LockVerticalCamera = 1 << 3,
            BetillaBorder = 1 << 4,
            Storm = 1 << 5,
            RainOrSnow_0 = 1 << 6,
            RainOrSnow_1 = 1 << 7,
            Wind = 1 << 8,
            Effect_9 = 1 << 9,
            HotEffect = 1 << 10,
            Effect_11 = 1 << 11,
            HideHUD = 1 << 12,
            Effect_13 = 1 << 13,
            Effect_14 = 1 << 14,
            Effect_15 = 1 << 15,
        }
    }
}