﻿namespace BinarySerializer.Ray1
{
    public abstract class Mapper_BaseEventDefinition
    {
        public string Name { get; set; }

        public string DESFile { get; set; }

        public byte DisplayPrio { get; set; }

        public string ETAFile { get; set; }

        public byte[] EventCommands { get; set; }

        public short XPosition { get; set; }

        public short YPosition { get; set; }

        public byte Etat { get; set; }

        public byte SubEtat { get; set; }

        public byte OffsetBX { get; set; }

        public byte OffsetBY { get; set; }

        public byte OffsetHY { get; set; }

        public byte FollowEnabled { get; set; }

        public byte FollowSprite { get; set; }

        public uint HitPoints { get; set; }

        public ObjType Type { get; set; }

        public byte HitSprite { get; set; }
    }
}