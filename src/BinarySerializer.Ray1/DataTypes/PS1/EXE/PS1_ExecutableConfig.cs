namespace BinarySerializer.Ray1
{
    public class PS1_ExecutableConfig
    {
        #region Static Properties and Methods

        public static PS1_ExecutableConfig PS1_EU => new PS1_ExecutableConfig(new FileTableInfo[]
        {
            new FileTableInfo(0x801c3d5c,3,PS1_FileType.img_file),
            new FileTableInfo(0x801c3dc8,2,PS1_FileType.ldr_file),
            new FileTableInfo(0x801c3e10,2,PS1_FileType.div_file),
            new FileTableInfo(0x801c3e58,0x12,PS1_FileType.vdo_file),
            new FileTableInfo(0x801c40e0,0x35,PS1_FileType.trk_file),
            new FileTableInfo(0x801c4854,3,PS1_FileType.lang_file),
            new FileTableInfo(0x801c48c0,5,PS1_FileType.pre_file),
            new FileTableInfo(0x801c4974,6,PS1_FileType.crd_file),
            new FileTableInfo(0x801c4a4c,6,PS1_FileType.gam_file),
            new FileTableInfo(0x801c4b24,6,PS1_FileType.vig_wld_file),
            new FileTableInfo(0x801c4bfc,6,PS1_FileType.wld_file),
            new FileTableInfo(0x801c4cd4,0x7e,PS1_FileType.map_file),
            new FileTableInfo(0x801c5e8c,0x1f,PS1_FileType.fnd_file),
            new FileTableInfo(0x801c62e8,7,PS1_FileType.vab_file),
            new FileTableInfo(0x801c63e4,7,PS1_FileType.big_file),
            new FileTableInfo(0x801c64e0,7,PS1_FileType.vab4sep_file),
            new FileTableInfo(0x801c65dc,2,PS1_FileType.filefxs),
            new FileTableInfo(0x801c6624,1,PS1_FileType.ini_file),
        });

        public static PS1_ExecutableConfig PS1_EUDemo => new PS1_ExecutableConfig(new FileTableInfo[]
        {
            new FileTableInfo(0x801c42dc,3,PS1_FileType.img_file),
            new FileTableInfo(0x801c4228,2,PS1_FileType.ldr_file),
            new FileTableInfo(0x801c44d4,2,PS1_FileType.div_file),
            new FileTableInfo(0x801c3fa0,0x12,PS1_FileType.vdo_file),
            new FileTableInfo(0x801c6118,0x35,PS1_FileType.trk_file),
            new FileTableInfo(0x801c451c,3,PS1_FileType.lang_file),
            new FileTableInfo(0x801c4348,5,PS1_FileType.pre_file),
            new FileTableInfo(0x801c43fc,6,PS1_FileType.crd_file),
            new FileTableInfo(0x801c4588,6,PS1_FileType.gam_file),
            new FileTableInfo(0x801c4660,6,PS1_FileType.vig_wld_file),
            new FileTableInfo(0x801c4a2c,6,PS1_FileType.wld_file),
            new FileTableInfo(0x801c4b04,0x7e,PS1_FileType.map_file),
            new FileTableInfo(0x801c5cbc,0x1f,PS1_FileType.fnd_file),
            new FileTableInfo(0x801c4834,7,PS1_FileType.vab_file),
            new FileTableInfo(0x801c4738,7,PS1_FileType.big_file),
            new FileTableInfo(0x801c4930,7,PS1_FileType.vab4sep_file),
            new FileTableInfo(0x801c4294,2,PS1_FileType.filefxs),
            new FileTableInfo(0x801c4270,1,PS1_FileType.ini_file),
        });

        public static PS1_ExecutableConfig PS1_JP => new PS1_ExecutableConfig(new FileTableInfo[]
        {
            new FileTableInfo(0x801c1770,3,PS1_FileType.img_file),
            new FileTableInfo(0x801c17dc,2,PS1_FileType.ldr_file),
            new FileTableInfo(0x801c1824,6,PS1_FileType.vdo_file),
            new FileTableInfo(0x801c18fc,0x31,PS1_FileType.trk_file),
            new FileTableInfo(0x801c1fe0,5,PS1_FileType.pre_file),
            new FileTableInfo(0x801c2094,6,PS1_FileType.crd_file),
            new FileTableInfo(0x801c216c,6,PS1_FileType.gam_file),
            new FileTableInfo(0x801c2244,6,PS1_FileType.vig_wld_file),
            new FileTableInfo(0x801c231c,6,PS1_FileType.wld_file),
            new FileTableInfo(0x801c23f4,0x7e,PS1_FileType.map_file),
            new FileTableInfo(0x801c35ac,8,PS1_FileType.blc_file),
            new FileTableInfo(0x801c36cc,0x1f,PS1_FileType.fnd_file),
            new FileTableInfo(0x801c3b28,6,PS1_FileType.vab_file),
            new FileTableInfo(0x801c3c00,2,PS1_FileType.filefxs),
            new FileTableInfo(0x801c3c48,1,PS1_FileType.ini_file),
        });

        public static PS1_ExecutableConfig PS1_JPDemoVol3 => new PS1_ExecutableConfig(new FileTableInfo[]
        {
            new FileTableInfo(0x801b5f3c, 1, PS1_FileType.pal_file),
            new FileTableInfo(0x801b5f74, 4, PS1_FileType.demo_vig),
            new FileTableInfo(0x801b6054, 1, PS1_FileType.img_file),
            new FileTableInfo(0x801b608c, 1, PS1_FileType.filefxs),
            new FileTableInfo(0x801b60c4, 0xb, PS1_FileType.demo_w1),
            new FileTableInfo(0x801b6594, 0xb, PS1_FileType.demo_w2),
            new FileTableInfo(0x801b67fc, 4, PS1_FileType.fnd_file),
            new FileTableInfo(0x801b68fc, 1, PS1_FileType.demo_track)
        });

        public static PS1_ExecutableConfig PS1_JPDemoVol6 => new PS1_ExecutableConfig(new FileTableInfo[]
        {
            new FileTableInfo(0x801b791c, 3, PS1_FileType.filefxs),
            new FileTableInfo(0x801b79d0, 1, PS1_FileType.demo_vig),
            new FileTableInfo(0x801b7a0c, 12, PS1_FileType.vab_file),

            new FileTableInfo(0x801b7cdc, 5, PS1_FileType.wld_file), // Jungle

            // Unused
            //new FileTableInfo(0x801B7E08, 5, R1_PS1_FileType.wld_file), // Music
            //new FileTableInfo(0x801B7F34, 5, R1_PS1_FileType.wld_file), // Mountain
            //new FileTableInfo(0x801B8060, 5, R1_PS1_FileType.wld_file), // Image
            //new FileTableInfo(0x801B818C, 5, R1_PS1_FileType.wld_file), // Cave
            //new FileTableInfo(0x801B82B8, 5, R1_PS1_FileType.wld_file), // Cake

            new FileTableInfo(0x801b83e4, 64, PS1_FileType.map_file), // Jungle

            // Unused
            //new FileTableInfo(0x801B92E4, 64, R1_PS1_FileType.map_file), // Music
            //new FileTableInfo(0x801BA1E4, 64, R1_PS1_FileType.map_file), // Mountain
            //new FileTableInfo(0x801BB0E4, 64, R1_PS1_FileType.map_file), // Image
            //new FileTableInfo(0x801BBFE4, 64, R1_PS1_FileType.map_file), // Cave
            //new FileTableInfo(0x801BCEE4, 64, R1_PS1_FileType.map_file), // Cake

            new FileTableInfo(0x801BDDE4, 11, PS1_FileType.fnd_file),
            new FileTableInfo(0x801be078, 11, PS1_FileType.demo_file), // BGI (background data?)

            new FileTableInfo(0x801BE30C, 4, PS1_FileType.trk_file)
        });

        public static PS1_ExecutableConfig PS1_US => new PS1_ExecutableConfig(new FileTableInfo[]
        {
            new FileTableInfo(0x801c4b38,3,PS1_FileType.img_file),
            new FileTableInfo(0x801c4ba4,2,PS1_FileType.ldr_file),
            new FileTableInfo(0x801c4bec,6,PS1_FileType.vdo_file),
            new FileTableInfo(0x801c4cc4,0x35,PS1_FileType.trk_file),
            new FileTableInfo(0x801c5438,5,PS1_FileType.pre_file),
            new FileTableInfo(0x801c54ec,6,PS1_FileType.crd_file),
            new FileTableInfo(0x801c55c4,6,PS1_FileType.gam_file),
            new FileTableInfo(0x801c569c,6,PS1_FileType.vig_wld_file),
            new FileTableInfo(0x801c5774,6,PS1_FileType.wld_file),
            new FileTableInfo(0x801c584c,0x7e,PS1_FileType.map_file),
            new FileTableInfo(0x801c6a04,0x1f,PS1_FileType.fnd_file),
            new FileTableInfo(0x801c6e60,7,PS1_FileType.vab_file),
            new FileTableInfo(0x801c6f5c,7,PS1_FileType.big_file),
            new FileTableInfo(0x801c7058,7,PS1_FileType.vab4sep_file),
            new FileTableInfo(0x801c7154,2,PS1_FileType.filefxs),
            new FileTableInfo(0x801c719c,1,PS1_FileType.ini_file),
        });

        public static PS1_ExecutableConfig PS1_USDemo => new PS1_ExecutableConfig(new FileTableInfo[]
        {
            new FileTableInfo(0x801c269c,3,PS1_FileType.img_file),
            new FileTableInfo(0x801c2708,2,PS1_FileType.ldr_file),
            new FileTableInfo(0x801c2750,6,PS1_FileType.vdo_file),
            new FileTableInfo(0x801c2828,0x35,PS1_FileType.trk_file),
            new FileTableInfo(0x801c2f9c,5,PS1_FileType.pre_file),
            new FileTableInfo(0x801c3050,6,PS1_FileType.crd_file),
            new FileTableInfo(0x801c3128,6,PS1_FileType.gam_file),
            new FileTableInfo(0x801c3200,6,PS1_FileType.vig_wld_file),
            new FileTableInfo(0x801c32d8,6,PS1_FileType.wld_file),
            new FileTableInfo(0x801c33b0,0x7e,PS1_FileType.map_file),
            new FileTableInfo(0x801c4568,0x1f,PS1_FileType.fnd_file ),
            new FileTableInfo(0x801c49c4,7,PS1_FileType.vab_file),
            new FileTableInfo(0x801c4ac0,7,PS1_FileType.big_file),
            new FileTableInfo(0x801c4bbc,7,PS1_FileType.vab4sep_file),
            new FileTableInfo(0x801c4cb8,2,PS1_FileType.filefxs),
            new FileTableInfo(0x801c4d00,1,PS1_FileType.ini_file),
        });

        #endregion

        #region Constructor

        public PS1_ExecutableConfig(FileTableInfo[] fileTableInfos)
        {
            FileTableInfos = fileTableInfos;
        }

        #endregion

        #region Public Properties

        public FileTableInfo[] FileTableInfos { get; }

        #endregion

        #region Classes

        public class FileTableInfo
        {
            public FileTableInfo(uint offset, uint count, PS1_FileType fileType)
            {
                Offset = offset;
                Count = count;
                FileType = fileType;
            }

            public uint Offset { get; }
            public uint Count { get; }
            public PS1_FileType FileType { get; }
        }

        #endregion
    }
}