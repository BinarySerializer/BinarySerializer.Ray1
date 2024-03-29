﻿namespace BinarySerializer.Ray1.PC
{
    public class ProfileDefine : BinarySerializable
    {
        /// <summary>
        /// The checksum for the encrypted data
        /// </summary>
        public byte ProfileDefineChecksum { get; set; }

        /// <summary>
        /// The KIT level name
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// The KIT level author
        /// </summary>
        public string LevelAuthor { get; set; }

        /// <summary>
        /// The KIT level description
        /// </summary>
        public string LevelDescription { get; set; }

        public bool Power_Fist { get; set; }
        public bool Power_Hang { get; set; }
        public bool Power_Run { get; set; }
        public bool Power_Seed { get; set; }
        public bool Power_Helico { get; set; }
        public bool Power_SuperHelico { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.DoProcessed(new Checksum8Processor(), p =>
            {
                p.Serialize<byte>(s, "ProfileDefineChecksum");

                s.DoProcessed(new Xor8Processor(0x96), () =>
                {
                    LevelName = s.SerializeString(LevelName, 25, name: nameof(LevelName));
                    LevelAuthor = s.SerializeString(LevelAuthor, 25, name: nameof(LevelAuthor));
                    LevelDescription = s.SerializeString(LevelDescription, 240, name: nameof(LevelDescription));

                    Power_Fist = s.Serialize<bool>(Power_Fist, name: nameof(Power_Fist));
                    Power_Hang = s.Serialize<bool>(Power_Hang, name: nameof(Power_Hang));
                    Power_Run = s.Serialize<bool>(Power_Run, name: nameof(Power_Run));
                    Power_Seed = s.Serialize<bool>(Power_Seed, name: nameof(Power_Seed));
                    Power_Helico = s.Serialize<bool>(Power_Helico, name: nameof(Power_Helico));
                    Power_SuperHelico = s.Serialize<bool>(Power_SuperHelico, name: nameof(Power_SuperHelico));
                });
            });
        }
    }
}