using System;

namespace BinarySerializer.Ray1
{
    public class Ray1Settings
    {
        public Ray1Settings(Ray1EngineVersion engineVersion, World world, int level, Ray1PCVersion pcVersion = Ray1PCVersion.None, string volume = null, bool isFan = false)
        {
            EngineVersion = engineVersion;
            World = world;
            Level = level;
            PCVersion = pcVersion;
            Volume = volume;
            IsFAN = isFan;

            switch (EngineVersion)
            {
                case Ray1EngineVersion.R1_PS1:
                case Ray1EngineVersion.R2_PS1:
                case Ray1EngineVersion.R1_PS1_JP:
                case Ray1EngineVersion.R1_PS1_JPDemoVol3:
                case Ray1EngineVersion.R1_PS1_JPDemoVol6:
                case Ray1EngineVersion.R1_Saturn:
                    EngineBranch = Ray1EngineBranch.PS1;
                    break;

                case Ray1EngineVersion.R1_PC:
                case Ray1EngineVersion.R1_PocketPC:
                case Ray1EngineVersion.R1_PC_Kit:
                case Ray1EngineVersion.R1_PC_Edu:
                case Ray1EngineVersion.R1_PS1_Edu:
                    EngineBranch = Ray1EngineBranch.PC;
                    break;

                case Ray1EngineVersion.R1_GBA:
                case Ray1EngineVersion.R1_DSi:
                    EngineBranch = Ray1EngineBranch.GBA;
                    break;

                case Ray1EngineVersion.R1Jaguar:
                case Ray1EngineVersion.R1Jaguar_Proto:
                case Ray1EngineVersion.R1Jaguar_Demo:
                    EngineBranch = Ray1EngineBranch.Jaguar;
                    break;

                case Ray1EngineVersion.SNES:
                    EngineBranch = Ray1EngineBranch.SNES;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(engineVersion), engineVersion, null);
            }
        }

        public Ray1EngineVersion EngineVersion { get; }
        public Ray1EngineBranch EngineBranch { get; }
        public Ray1PCVersion PCVersion { get; }
        public World World { get; }
        public int Level { get; }
        public string Volume { get; }
        public bool IsFAN { get; } // TODO: Replace by engine version tree

        public static int CellSize = 16;
    }

    public enum Ray1EngineVersion
    {
        R1_PS1,
        R2_PS1,
        R1_PS1_JP,
        R1_PS1_JPDemoVol3,
        R1_PS1_JPDemoVol6,
        R1_Saturn,
        R1_PC,
        R1_PocketPC,
        R1_PC_Kit,
        R1_PC_Edu,
        R1_PS1_Edu,
        R1_GBA,
        R1_DSi,

        R1Jaguar,
        R1Jaguar_Proto,
        R1Jaguar_Demo,

        SNES,
    }

    public enum Ray1EngineBranch
    {
        SNES,
        Jaguar,
        PS1,
        PC,
        GBA,
    }

    public enum Ray1PCVersion
    {
        None,
        PC_1_00,
        PC_1_10,
        PC_1_12,
        PC_1_20,
        PC_1_21_JP,
        PC_1_21,
        PC_Demo_1,
        PC_Demo_2,
        PocketPC,
        Android,
        iOS,
    }
}
