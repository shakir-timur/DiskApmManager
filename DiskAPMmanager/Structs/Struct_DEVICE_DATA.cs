using System.Runtime.InteropServices;

namespace DiskAPMmanager.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IDENTIFY_DEVICE_DATA
    {
        public ushort GeneralConfig_000;
        public ushort word_001;
        public ushort word_002;
        public ushort word_003;
        public ushort word_004;
        public ushort word_005;
        public ushort word_006;
        public ushort word_007;
        public ushort word_008;
        public ushort word_009; 

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] serial_no;       /* word_010 - word_019. 0 = not_specified */

        public ushort word_020;
        public ushort word_021;
        public ushort word_022;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] fw_rev;          /* word_023 - word_026. 0 = not_specified */

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] model;           /* word_027 - word_046. 0 = not_specified */

        public ushort word_047;
        public ushort word_048;
        public ushort Capabilities_049;
        public ushort Capabilities_050;
        public ushort word_051;
        public ushort word_052;
        public ushort word_053;
        public ushort word_054;
        public ushort word_055;
        public ushort word_056;
        public ushort word_057;
        public ushort word_058;
        public ushort word_059;
        public ushort word_060;
        public ushort word_061;
        public ushort word_062;
        public ushort word_063;
        public ushort word_064;
        public ushort word_065;
        public ushort word_066;
        public ushort word_067;
        public ushort word_068;
        public ushort word_069;
        public ushort word_070;
        public ushort word_071;
        public ushort word_072;
        public ushort word_073;
        public ushort word_074;
        public ushort word_075;
        public ushort SATA_Capabilities_076;
        public ushort SATA_Addit_Capabil_077;

        public ushort SATA_features_supported_078;
        public ushort SATA_features_enabled_079;

        public ushort Major_version_no_080;

        public ushort word_081;

        public ushort Feature_set_support_082;
        public ushort Feature_set_support_083;
        public ushort Feature_set_support_084;
        public ushort Feature_set_enabled_085;
        public ushort Feature_set_enabled_086;
        public ushort Feature_set_enabled_087;

        public ushort word_088;
        public ushort word_089;
        public ushort word_090;

        public byte CurrentAPMvalue;
        public byte word_091_bytes_15_8; // Reserved

        public ushort word_092;
        public ushort word_093;
        public ushort word_094;
        public ushort word_095;
        public ushort word_096;
        public ushort word_097;
        public ushort word_098;
        public ushort word_099;
        public ushort word_100;
        public ushort word_101;
        public ushort word_102;
        public ushort word_103;
        public ushort word_104;
        public ushort word_105;
        public ushort word_106;
        public ushort word_107;
        public ushort word_108;
        public ushort word_109;
        public ushort word_110;
        public ushort word_111;
        public ushort word_112;
        public ushort word_113;
        public ushort word_114;
        public ushort word_115;
        public ushort word_116;
        public ushort word_117;
        public ushort word_118;
        public ushort Feature_set_support_119;
        public ushort Feature_set_enabled_120;
        public ushort word_121;
        public ushort word_122;
        public ushort word_123;
        public ushort word_124;
        public ushort word_125;
        public ushort word_126;
        public ushort word_127;
        public ushort word_128;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
        public ushort[] Vendor_specific_word_129_159; 

        public ushort word_160;
        public ushort word_161;
        public ushort word_162;
        public ushort word_163;
        public ushort word_164;
        public ushort word_165;
        public ushort word_166;
        public ushort word_167;
        public ushort word_168;
        public ushort word_169;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] add_prod_id;        // Words 170..173: Additional Product Identifier

        public ushort word_174;
        public ushort word_175;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] Cur_Media_Serial_No;  /* Current Media Serial Number. Was not filled with data on my disk */

        public ushort word_206;
        public ushort word_207;
        public ushort word_208;
        public ushort word_209;
        public ushort word_210;
        public ushort word_211;
        public ushort word_212;
        public ushort word_213;
        public ushort word_214;
        public ushort word_215;
        public ushort word_216;

        //0401h-FFFEh Nominal media rotation rate in rotations per minute (rpm) (e.g., 7 200 rpm = 1C20h). 1 for non-rotative
        public ushort NomMediaRotRate_217;

        public ushort word_218;
        public ushort word_219;
        public ushort word_220;
        public ushort word_221;
        public ushort word_222;
        public ushort word_223;
        public ushort word_224;
        public ushort word_225;
        public ushort word_226;
        public ushort word_227;
        public ushort word_228;
        public ushort word_229;
        public ushort word_230;
        public ushort word_231;
        public ushort word_232;
        public ushort word_233;
        public ushort word_234;
        public ushort word_235;
        public ushort word_236;
        public ushort word_237;
        public ushort word_238;
        public ushort word_239;
        public ushort word_240;
        public ushort word_241;
        public ushort word_242;
        public ushort word_243;
        public ushort word_244;
        public ushort word_245;
        public ushort word_246;
        public ushort word_247;
        public ushort word_248;
        public ushort word_249;
        public ushort word_250;
        public ushort word_251;
        public ushort word_252;
        public ushort word_253;
        public ushort word_254;
        public ushort word_255;
    }
}