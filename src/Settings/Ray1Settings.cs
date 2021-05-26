using System;
using System.Text;

namespace BinarySerializer.Ray1
{
    public class Ray1Settings
    {
        public Ray1Settings(Ray1EngineVersion engineVersion, World world, int level, Ray1PCVersion pcVersion = Ray1PCVersion.None, string volume = null)
        {
            EngineVersion = engineVersion;
            World = world;
            Level = level;
            PCVersion = pcVersion;
            Volume = volume;

            switch (EngineVersion)
            {
                case Ray1EngineVersion.PS1:
                case Ray1EngineVersion.R2_PS1:
                case Ray1EngineVersion.PS1_JP:
                case Ray1EngineVersion.PS1_JPDemoVol3:
                case Ray1EngineVersion.PS1_JPDemoVol6:
                case Ray1EngineVersion.Saturn:
                    EngineBranch = Ray1EngineBranch.PS1;
                    break;

                case Ray1EngineVersion.PC:
                case Ray1EngineVersion.PocketPC:
                case Ray1EngineVersion.PC_Kit:
                case Ray1EngineVersion.PC_Fan:
                case Ray1EngineVersion.PC_Edu:
                case Ray1EngineVersion.PS1_Edu:
                    EngineBranch = Ray1EngineBranch.PC;
                    break;

                case Ray1EngineVersion.GBA:
                case Ray1EngineVersion.DSi:
                    EngineBranch = Ray1EngineBranch.GBA;
                    break;

                case Ray1EngineVersion.Jaguar:
                case Ray1EngineVersion.Jaguar_Proto:
                case Ray1EngineVersion.Jaguar_Demo:
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

        public World World { get; set; }
        public int Level { get; set; }
        public string Volume { get; set; }

        public static int CellSize = 16;
        public static Encoding DefaultEncoding => Encoding.GetEncoding(437);
    }
}
