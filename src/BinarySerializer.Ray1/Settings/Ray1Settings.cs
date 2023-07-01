using System;
using System.Text;

namespace BinarySerializer.Ray1
{
    public class Ray1Settings
    {
        public Ray1Settings(Ray1EngineVersion engineVersion, World world = World.Jungle, int level = 1, Ray1PCVersion pcVersion = Ray1PCVersion.None, string volume = null)
        {
            EngineVersion = engineVersion;
            World = world;
            Level = level;
            PCVersion = pcVersion;
            Volume = volume;

            switch (EngineVersion)
            {
                case Ray1EngineVersion.PS1:
                case Ray1EngineVersion.PS1_EUDemo:
                case Ray1EngineVersion.R2_PS1:
                case Ray1EngineVersion.PS1_JP:
                case Ray1EngineVersion.PS1_JPDemoVol3:
                case Ray1EngineVersion.PS1_JPDemoVol6:
                case Ray1EngineVersion.Saturn:
                    EnginePlatform = Ray1EnginePlatform.PS1_Saturn;
                    break;

                case Ray1EngineVersion.PC:
                case Ray1EngineVersion.PocketPC:
                case Ray1EngineVersion.PC_Kit:
                case Ray1EngineVersion.PC_Fan:
                case Ray1EngineVersion.PC_Edu:
                case Ray1EngineVersion.PS1_Edu:
                    EnginePlatform = Ray1EnginePlatform.PC;
                    break;

                case Ray1EngineVersion.GBA:
                case Ray1EngineVersion.DSi:
                    EnginePlatform = Ray1EnginePlatform.GBA_DSi;
                    break;

                case Ray1EngineVersion.Jaguar:
                case Ray1EngineVersion.Jaguar_Proto:
                case Ray1EngineVersion.Jaguar_Demo:
                    EnginePlatform = Ray1EnginePlatform.Jaguar;
                    break;

                case Ray1EngineVersion.SNES:
                    EnginePlatform = Ray1EnginePlatform.SNES;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(engineVersion), engineVersion, null);
            }

            EngineVersionTree = CreateEngineVersionTree();
            IsVersioned = EngineVersion == Ray1EngineVersion.PC_Kit ||
                          EngineVersion == Ray1EngineVersion.PC_Edu ||
                          EngineVersion == Ray1EngineVersion.PS1_Edu ||
                          EngineVersion == Ray1EngineVersion.PC_Fan;
            IsLoadingPackedPCData = EnginePlatform == Ray1EnginePlatform.PC;
        }

        public Ray1EngineVersion EngineVersion { get; }
        public Ray1EnginePlatform EnginePlatform { get; }
        public Ray1PCVersion PCVersion { get; }

        public VersionTree<Ray1EngineVersion> EngineVersionTree { get; }

        public bool IsVersioned { get; }

        public World World { get; set; }
        public int Level { get; set; }
        public string Volume { get; set; }

        /// <summary>
        /// This determines how the data is to be serialized. For most platforms the data is already unpacked, such
        /// as PS1 and Saturn. However on PC the data is packed in the files and then unpacked into memory.
        /// </summary>
        public bool IsLoadingPackedPCData { get; set; }

        public static Encoding DefaultEncoding => Encoding.GetEncoding(437);

        private VersionTree<Ray1EngineVersion> CreateEngineVersionTree()
        {
            VersionTree<Ray1EngineVersion> tree = new(new VersionTree<Ray1EngineVersion>.Node(Ray1EngineVersion.ENGINE_ROOT)
            {
                // SNES
                new(Ray1EngineVersion.ENGINE_SNES) {
                    new(Ray1EngineVersion.SNES)
                },

                // Jaguar
                new(Ray1EngineVersion.ENGINE_JAGUAR) {
                    new(Ray1EngineVersion.Jaguar_Proto) {
                        new(Ray1EngineVersion.Jaguar_Demo) {
                            new(Ray1EngineVersion.Jaguar)
                        }
                    }
                },

                // Multiplat
                new(Ray1EngineVersion.ENGINE_MULTIPLAT) {
                    new(Ray1EngineVersion.PS1_JPDemoVol3) {
                        new(Ray1EngineVersion.PS1_JPDemoVol6) {
                            new(Ray1EngineVersion.PS1_JP) {
                                new(Ray1EngineVersion.Saturn) {
                                    new(Ray1EngineVersion.PS1) {
                                        new(Ray1EngineVersion.PS1_EUDemo) {
                                            new(Ray1EngineVersion.PC) {
                                                new(Ray1EngineVersion.PocketPC),
                                                new(Ray1EngineVersion.GBA) {
                                                    new(Ray1EngineVersion.DSi)
                                                },
                                                new(Ray1EngineVersion.PC_Edu) {
                                                    new(Ray1EngineVersion.PS1_Edu),
                                                    new(Ray1EngineVersion.PC_Kit) {
                                                        new(Ray1EngineVersion.PC_Fan),
                                                    },
                                                    new(Ray1EngineVersion.R2_PS1),
                                                },
                                            }
                                        },
                                    }
                                }
                            }
                        }
                    }
                },
            });
            tree.Init();
            tree.Current = tree.FindVersion(EngineVersion);

            return tree;
        }
    }
}
