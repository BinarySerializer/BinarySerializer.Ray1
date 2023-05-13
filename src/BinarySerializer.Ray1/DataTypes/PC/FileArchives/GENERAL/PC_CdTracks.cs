﻿namespace BinarySerializer.Ray1
{
    public class PC_CdTracks : BinarySerializable
    {
        public byte? LogoTrack { get; set; }
        public byte? BigRayTrack { get; set; }
        public byte? MenuTrack { get; set; }
        public byte? CreditsTrack { get; set; }
        public byte? ContinueTrack { get; set; }
        public byte? Track_05 { get; set; }
        public byte?[] LevelTracks { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            LogoTrack = s.SerializeNullable<byte>(LogoTrack, name: nameof(LogoTrack));
            BigRayTrack = s.SerializeNullable<byte>(BigRayTrack, name: nameof(BigRayTrack));
            MenuTrack = s.SerializeNullable<byte>(MenuTrack, name: nameof(MenuTrack));
            CreditsTrack = s.SerializeNullable<byte>(CreditsTrack, name: nameof(CreditsTrack));
            ContinueTrack = s.SerializeNullable<byte>(ContinueTrack, name: nameof(ContinueTrack));
            Track_05 = s.SerializeNullable<byte>(Track_05, name: nameof(Track_05));

            int levelTracksCount = 21;

            // The PS1 version hard-codes a different length for this version
            if (settings.EngineVersion == Ray1EngineVersion.PS1_Edu && settings.Volume.StartsWith("CS"))
                levelTracksCount = 23;

            LevelTracks = s.SerializeNullableArray<byte>(LevelTracks, levelTracksCount, name: nameof(LevelTracks));
        }
    }
}