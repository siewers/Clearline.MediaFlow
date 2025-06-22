namespace Clearline.MediaFlow;

// ReSharper disable InconsistentNaming
public readonly partial record struct VideoCodec
{
    /// <summary>
    ///     Copy the input video stream without re-encoding.
    /// </summary>
    public static VideoCodec Copy { get; } = new("copy");

    /// <summary>
    ///     Uncompressed 4:2:2 10-bit
    /// </summary>
    public static VideoCodec _012v { get; } = new("012v");

    /// <summary>
    ///     4X Movie
    /// </summary>
    public static VideoCodec _4xm { get; } = new("4xm");

    /// <summary>
    ///     QuickTime 8BPS video
    /// </summary>
    public static VideoCodec _8bps { get; } = new("8bps");

    /// <summary>
    ///     libx264
    /// </summary>
    public static VideoCodec Libx264 { get; } = new("libx264");

    /// <summary>
    ///     MPEG-4 part 2
    /// </summary>
    public static VideoCodec Mpeg4 { get; } = new("mpeg4");

    /// <summary>
    ///     H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10
    /// </summary>
    public static VideoCodec h264 { get; } = new("h264");

    /// <summary>
    ///     Nvidia CUVID H.264 decoder
    /// </summary>
    public static VideoCodec h264_cuvid { get; } = new("h264_cuvid");

    /// <summary>
    ///     Nvidia NVENC H.264 encoder
    /// </summary>
    public static VideoCodec h264_nvenc { get; } = new("h264_nvenc");

    /// <summary>
    ///     PNG (Portable Network Graphics) image
    /// </summary>
    public static VideoCodec png => new("png");

    /// <summary>
    ///     On2 VP8
    /// </summary>
    public static VideoCodec vp8 { get; } = new("vp8");

    /// <summary>
    ///     Theora
    /// </summary>
    public static VideoCodec Theora { get; } = new("theora");

    /// <summary>
    ///     MPEG-2 video
    /// </summary>
    public static VideoCodec Mpeg2Video { get; } = new("mpeg2video");

    /// <summary>
    ///     H.265 / HEVC (High Efficiency Video Coding)
    /// </summary>
    public static VideoCodec hevc { get; } = new("hevc");

    /// <summary>
    ///     Multicolor charset for Commodore 64
    /// </summary>
    public static VideoCodec A64Multi { get; } = new("a64_multi");

    /// <summary>
    ///     Multicolor charset for Commodore 64, extended with 5th color (colram)
    /// </summary>
    public static VideoCodec A64Multi5 { get; } = new("a64_multi5");

    /// <summary>
    ///     Autodesk RLE
    /// </summary>
    public static VideoCodec Aasc { get; } = new("aasc");

    /// <summary>
    ///     Amuse Graphics Movie
    /// </summary>
    public static VideoCodec Agm { get; } = new("agm");

    /// <summary>
    ///     Apple Intermediate Codec
    /// </summary>
    public static VideoCodec Aic { get; } = new("aic");

    /// <summary>
    ///     Alias/Wavefront PIX image
    /// </summary>
    public static VideoCodec AliasPix { get; } = new("alias_pix");

    /// <summary>
    ///     AMV Video
    /// </summary>
    public static VideoCodec Amv { get; } = new("amv");

    /// <summary>
    ///     Deluxe Paint Animation
    /// </summary>
    public static VideoCodec Anm { get; } = new("anm");

    /// <summary>
    ///     ASCII/ANSI art
    /// </summary>
    public static VideoCodec Ansi { get; } = new("ansi");

    /// <summary>
    ///     APNG (Animated Portable Network Graphics) image
    /// </summary>
    public static VideoCodec Apng { get; } = new("apng");

    /// <summary>
    ///     Gryphon's Anim Compressor
    /// </summary>
    public static VideoCodec Arbc { get; } = new("arbc");

    /// <summary>
    ///     ASUS V1
    /// </summary>
    public static VideoCodec Asv1 { get; } = new("asv1");

    /// <summary>
    ///     ASUS V2
    /// </summary>
    public static VideoCodec Asv2 { get; } = new("asv2");

    /// <summary>
    ///     Auravision AURA
    /// </summary>
    public static VideoCodec Aura { get; } = new("aura");

    /// <summary>
    ///     Auravision Aura 2
    /// </summary>
    public static VideoCodec Aura2 { get; } = new("aura2");

    /// <summary>
    ///     Alliance for Open Media AV1
    /// </summary>
    public static VideoCodec Av1 { get; } = new("av1");

    /// <summary>
    ///     Avid AVI Codec
    /// </summary>
    public static VideoCodec Avrn { get; } = new("avrn");

    /// <summary>
    ///     Avid 1:1 10-bit RGB Packer
    /// </summary>
    public static VideoCodec Avrp { get; } = new("avrp");

    /// <summary>
    ///     AVS (Audio Video Standard) video
    /// </summary>
    public static VideoCodec Avs { get; } = new("avs");

    /// <summary>
    ///     AVS2-P2/IEEE1857.4
    /// </summary>
    public static VideoCodec Avs2 { get; } = new("avs2");

    /// <summary>
    ///     Avid Meridien Uncompressed
    /// </summary>
    public static VideoCodec Avui { get; } = new("avui");

    /// <summary>
    ///     Uncompressed packed MS 4:4:4:4
    /// </summary>
    public static VideoCodec Ayuv { get; } = new("ayuv");

    /// <summary>
    ///     Bethesda VID video
    /// </summary>
    public static VideoCodec Bethsoftvid { get; } = new("bethsoftvid");

    /// <summary>
    ///     Brute Force and Ignorance
    /// </summary>
    public static VideoCodec Bfi { get; } = new("bfi");

    /// <summary>
    ///     Bink video
    /// </summary>
    public static VideoCodec Binkvideo { get; } = new("binkvideo");

    /// <summary>
    ///     Binary text
    /// </summary>
    public static VideoCodec Bintext { get; } = new("bintext");

    /// <summary>
    ///     Bitpacked
    /// </summary>
    public static VideoCodec Bitpacked { get; } = new("bitpacked");

    /// <summary>
    ///     BMP (Windows and OS/2 bitmap)
    /// </summary>
    public static VideoCodec Bmp { get; } = new("bmp");

    /// <summary>
    ///     Discworld II BMV video
    /// </summary>
    public static VideoCodec BmvVideo { get; } = new("bmv_video");

    /// <summary>
    ///     BRender PIX image
    /// </summary>
    public static VideoCodec BrenderPix { get; } = new("brender_pix");

    /// <summary>
    ///     Interplay C93
    /// </summary>
    public static VideoCodec C93 { get; } = new("c93");

    /// <summary>
    ///     Chinese AVS (Audio Video Standard) (AVS1-P2, JiZhun profile)
    /// </summary>
    public static VideoCodec Cavs { get; } = new("cavs");

    /// <summary>
    ///     CD Graphics video
    /// </summary>
    public static VideoCodec Cdgraphics { get; } = new("cdgraphics");

    /// <summary>
    ///     Commodore CDXL video
    /// </summary>
    public static VideoCodec Cdxl { get; } = new("cdxl");

    /// <summary>
    ///     Cineform HD
    /// </summary>
    public static VideoCodec cfhd { get; } = new("cfhd");

    /// <summary>
    ///     Cinepak
    /// </summary>
    public static VideoCodec cinepak { get; } = new("cinepak");

    /// <summary>
    ///     Iterated Systems ClearVideo
    /// </summary>
    public static VideoCodec clearvideo { get; } = new("clearvideo");

    /// <summary>
    ///     Cirrus Logic AccuPak
    /// </summary>
    public static VideoCodec cljr { get; } = new("cljr");

    /// <summary>
    ///     Canopus Lossless Codec
    /// </summary>
    public static VideoCodec cllc { get; } = new("cllc");

    /// <summary>
    ///     Electronic Arts CMV video
    /// </summary>
    public static VideoCodec cmv { get; } = new("cmv");

    /// <summary>
    ///     CPiA video format
    /// </summary>
    public static VideoCodec cpia { get; } = new("cpia");

    /// <summary>
    ///     CamStudio
    /// </summary>
    public static VideoCodec cscd { get; } = new("cscd");

    /// <summary>
    ///     Creative YUV (CYUV)
    /// </summary>
    public static VideoCodec cyuv { get; } = new("cyuv");

    /// <summary>
    ///     Daala
    /// </summary>
    public static VideoCodec daala { get; } = new("daala");

    /// <summary>
    ///     DirectDraw Surface image decoder
    /// </summary>
    public static VideoCodec dds { get; } = new("dds");

    /// <summary>
    ///     Chronomaster DFA
    /// </summary>
    public static VideoCodec dfa { get; } = new("dfa");

    /// <summary>
    ///     Dirac
    /// </summary>
    public static VideoCodec dirac { get; } = new("dirac");

    /// <summary>
    ///     VC3/DNxHD
    /// </summary>
    public static VideoCodec dnxhd { get; } = new("dnxhd");

    /// <summary>
    ///     DPX (Digital Picture Exchange) image
    /// </summary>
    public static VideoCodec dpx { get; } = new("dpx");

    /// <summary>
    ///     Delphine Software International CIN video
    /// </summary>
    public static VideoCodec dsicinvideo { get; } = new("dsicinvideo");

    /// <summary>
    ///     DV (Digital Video)
    /// </summary>
    public static VideoCodec dvvideo { get; } = new("dvvideo");

    /// <summary>
    ///     Feeble Files/ScummVM DXA
    /// </summary>
    public static VideoCodec dxa { get; } = new("dxa");

    /// <summary>
    ///     Dxtory
    /// </summary>
    public static VideoCodec dxtory { get; } = new("dxtory");

    /// <summary>
    ///     Resolume DXV
    /// </summary>
    public static VideoCodec dxv { get; } = new("dxv");

    /// <summary>
    ///     Escape 124
    /// </summary>
    public static VideoCodec escape124 { get; } = new("escape124");

    /// <summary>
    ///     Escape 130
    /// </summary>
    public static VideoCodec escape130 { get; } = new("escape130");

    /// <summary>
    ///     OpenEXR image
    /// </summary>
    public static VideoCodec exr { get; } = new("exr");

    /// <summary>
    ///     FFmpeg video codec #1
    /// </summary>
    public static VideoCodec ffv1 { get; } = new("ffv1");

    /// <summary>
    ///     Huffyuv FFmpeg variant
    /// </summary>
    public static VideoCodec ffvhuff { get; } = new("ffvhuff");

    /// <summary>
    ///     Mirillis FIC
    /// </summary>
    public static VideoCodec fic { get; } = new("fic");

    /// <summary>
    ///     FITS (Flexible Image Transport System)
    /// </summary>
    public static VideoCodec fits { get; } = new("fits");

    /// <summary>
    ///     Flash Screen Video v1
    /// </summary>
    public static VideoCodec flashsv { get; } = new("flashsv");

    /// <summary>
    ///     Flash Screen Video v2
    /// </summary>
    public static VideoCodec flashsv2 { get; } = new("flashsv2");

    /// <summary>
    ///     Autodesk Animator Flic video
    /// </summary>
    public static VideoCodec flic { get; } = new("flic");

    /// <summary>
    ///     FLV / Sorenson Spark / Sorenson H.263 (Flash Video)
    /// </summary>
    public static VideoCodec flv1 { get; } = new("flv1");

    /// <summary>
    ///     FM Screen Capture Codec
    /// </summary>
    public static VideoCodec fmvc { get; } = new("fmvc");

    /// <summary>
    ///     Fraps
    /// </summary>
    public static VideoCodec fraps { get; } = new("fraps");

    /// <summary>
    ///     Forward Uncompressed
    /// </summary>
    public static VideoCodec frwu { get; } = new("frwu");

    /// <summary>
    ///     Go2Meeting
    /// </summary>
    public static VideoCodec g2m { get; } = new("g2m");

    /// <summary>
    ///     Gremlin Digital Video
    /// </summary>
    public static VideoCodec gdv { get; } = new("gdv");

    /// <summary>
    ///     CompuServe GIF (Graphics Interchange Format)
    /// </summary>
    public static VideoCodec gif { get; } = new("gif");

    /// <summary>
    ///     H.261
    /// </summary>
    public static VideoCodec h261 { get; } = new("h261");

    /// <summary>
    ///     H.263 / H.263-1996, H.263+ / H.263-1998 / H.263 version 2
    /// </summary>
    public static VideoCodec h263 { get; } = new("h263");

    /// <summary>
    ///     Intel H.263
    /// </summary>
    public static VideoCodec h263i { get; } = new("h263i");

    /// <summary>
    ///     H.263+ / H.263-1998 / H.263 version 2
    /// </summary>
    public static VideoCodec h263p { get; } = new("h263p");

    /// <summary>
    ///     Vidvox Hap
    /// </summary>
    public static VideoCodec hap { get; } = new("hap");

    /// <summary>
    ///     HNM 4 video
    /// </summary>
    public static VideoCodec hnm4video { get; } = new("hnm4video");

    /// <summary>
    ///     Canopus HQ/HQA
    /// </summary>
    public static VideoCodec hq_hqa { get; } = new("hq_hqa");

    /// <summary>
    ///     Canopus HQX
    /// </summary>
    public static VideoCodec Hqx { get; } = new("hqx");

    /// <summary>
    ///     HuffYUV
    /// </summary>
    public static VideoCodec Huffyuv { get; } = new("huffyuv");

    /// <summary>
    ///     HuffYUV MT
    /// </summary>
    public static VideoCodec Hymt { get; } = new("hymt");

    /// <summary>
    ///     id Quake II CIN video
    /// </summary>
    public static VideoCodec Idcin { get; } = new("idcin");

    /// <summary>
    ///     iCEDraw text
    /// </summary>
    public static VideoCodec Idf { get; } = new("idf");

    /// <summary>
    ///     IFF ACBM/ANIM/DEEP/ILBM/PBM/RGB8/RGBN
    /// </summary>
    public static VideoCodec IffIlbm { get; } = new("iff_ilbm");

    /// <summary>
    ///     Infinity IMM4
    /// </summary>
    public static VideoCodec Imm4 { get; } = new("imm4");

    /// <summary>
    ///     Intel Indeo 2
    /// </summary>
    public static VideoCodec Indeo2 { get; } = new("indeo2");

    /// <summary>
    ///     Intel Indeo 3
    /// </summary>
    public static VideoCodec Indeo3 { get; } = new("indeo3");

    /// <summary>
    ///     Intel Indeo Video Interactive 4
    /// </summary>
    public static VideoCodec Indeo4 { get; } = new("indeo4");

    /// <summary>
    ///     Intel Indeo Video Interactive 5
    /// </summary>
    public static VideoCodec Indeo5 { get; } = new("indeo5");

    /// <summary>
    ///     Interplay MVE video
    /// </summary>
    public static VideoCodec Interplayvideo { get; } = new("interplayvideo");

    /// <summary>
    ///     JPEG 2000
    /// </summary>
    public static VideoCodec Jpeg2000 { get; } = new("jpeg2000");

    /// <summary>
    ///     JPEG-LS
    /// </summary>
    public static VideoCodec Jpegls { get; } = new("jpegls");

    /// <summary>
    ///     Bitmap Brothers JV video
    /// </summary>
    public static VideoCodec Jv { get; } = new("jv");

    /// <summary>
    ///     Kega Game Video
    /// </summary>
    public static VideoCodec Kgv1 { get; } = new("kgv1");

    /// <summary>
    ///     Karl Morton's video codec
    /// </summary>
    public static VideoCodec Kmvc { get; } = new("kmvc");

    /// <summary>
    ///     Lagarith lossless
    /// </summary>
    public static VideoCodec Lagarith { get; } = new("lagarith");

    /// <summary>
    ///     Lossless JPEG
    /// </summary>
    public static VideoCodec Ljpeg { get; } = new("ljpeg");

    /// <summary>
    ///     LOCO
    /// </summary>
    public static VideoCodec Loco { get; } = new("loco");

    /// <summary>
    ///     LEAD Screen Capture
    /// </summary>
    public static VideoCodec Lscr { get; } = new("lscr");

    /// <summary>
    ///     Matrox Uncompressed SD
    /// </summary>
    public static VideoCodec M101 { get; } = new("m101");

    /// <summary>
    ///     Electronic Arts Madcow Video
    /// </summary>
    public static VideoCodec Mad { get; } = new("mad");

    /// <summary>
    ///     MagicYUV video
    /// </summary>
    public static VideoCodec Magicyuv { get; } = new("magicyuv");

    /// <summary>
    ///     Sony PlayStation MDEC (Motion DECoder)
    /// </summary>
    public static VideoCodec Mdec { get; } = new("mdec");

    /// <summary>
    ///     Mimic
    /// </summary>
    public static VideoCodec Mimic { get; } = new("mimic");

    /// <summary>
    ///     Motion JPEG
    /// </summary>
    public static VideoCodec Mjpeg { get; } = new("mjpeg");

    /// <summary>
    ///     Apple MJPEG-B
    /// </summary>
    public static VideoCodec Mjpegb { get; } = new("mjpegb");

    /// <summary>
    ///     American Laser Games MM Video
    /// </summary>
    public static VideoCodec Mmvideo { get; } = new("mmvideo");

    /// <summary>
    ///     Motion Pixels video
    /// </summary>
    public static VideoCodec Motionpixels { get; } = new("motionpixels");

    /// <summary>
    ///     MPEG-1 video
    /// </summary>
    public static VideoCodec Mpeg1Video { get; } = new("mpeg1video");

    /// <summary>
    ///     MS ATC Screen
    /// </summary>
    public static VideoCodec Msa1 { get; } = new("msa1");

    /// <summary>
    ///     Mandsoft Screen Capture Codec
    /// </summary>
    public static VideoCodec Mscc { get; } = new("mscc");

    /// <summary>
    ///     MPEG-4 part 2 Microsoft variant version 1
    /// </summary>
    public static VideoCodec Msmpeg4V1 { get; } = new("msmpeg4v1");

    /// <summary>
    ///     MPEG-4 part 2 Microsoft variant version 2
    /// </summary>
    public static VideoCodec Msmpeg4V2 { get; } = new("msmpeg4v2");

    /// <summary>
    ///     MPEG-4 part 2 Microsoft variant version 3
    /// </summary>
    public static VideoCodec Msmpeg4V3 { get; } = new("msmpeg4v3");

    /// <summary>
    ///     Microsoft RLE
    /// </summary>
    public static VideoCodec Msrle { get; } = new("msrle");

    /// <summary>
    ///     MS Screen 1
    /// </summary>
    public static VideoCodec Mss1 { get; } = new("mss1");

    /// <summary>
    ///     MS Windows Media Video V9 Screen
    /// </summary>
    public static VideoCodec Mss2 { get; } = new("mss2");

    /// <summary>
    ///     Microsoft Video 1
    /// </summary>
    public static VideoCodec Msvideo1 { get; } = new("msvideo1");

    /// <summary>
    ///     LCL (LossLess Codec Library) MSZH
    /// </summary>
    public static VideoCodec Mszh { get; } = new("mszh");

    /// <summary>
    ///     MS Expression Encoder Screen
    /// </summary>
    public static VideoCodec Mts2 { get; } = new("mts2");

    /// <summary>
    ///     Silicon Graphics Motion Video Compressor 1
    /// </summary>
    public static VideoCodec Mvc1 { get; } = new("mvc1");

    /// <summary>
    ///     Silicon Graphics Motion Video Compressor 2
    /// </summary>
    public static VideoCodec Mvc2 { get; } = new("mvc2");

    /// <summary>
    ///     MatchWare Screen Capture Codec
    /// </summary>
    public static VideoCodec Mwsc { get; } = new("mwsc");

    /// <summary>
    ///     Mobotix MxPEG video
    /// </summary>
    public static VideoCodec Mxpeg { get; } = new("mxpeg");

    /// <summary>
    ///     NuppelVideo/RTJPEG
    /// </summary>
    public static VideoCodec Nuv { get; } = new("nuv");

    /// <summary>
    ///     Amazing Studio Packed Animation File Video
    /// </summary>
    public static VideoCodec PafVideo { get; } = new("paf_video");

    /// <summary>
    ///     PAM (Portable AnyMap) image
    /// </summary>
    public static VideoCodec Pam { get; } = new("pam");

    /// <summary>
    ///     PBM (Portable BitMap) image
    /// </summary>
    public static VideoCodec Pbm { get; } = new("pbm");

    /// <summary>
    ///     PC Paintbrush PCX image
    /// </summary>
    public static VideoCodec Pcx { get; } = new("pcx");

    /// <summary>
    ///     PGM (Portable GrayMap) image
    /// </summary>
    public static VideoCodec Pgm { get; } = new("pgm");

    /// <summary>
    ///     PGMYUV (Portable GrayMap YUV) image
    /// </summary>
    public static VideoCodec Pgmyuv { get; } = new("pgmyuv");

    /// <summary>
    ///     Pictor/PC Paint
    /// </summary>
    public static VideoCodec Pictor { get; } = new("pictor");

    /// <summary>
    ///     Apple Pixlet
    /// </summary>
    public static VideoCodec Pixlet { get; } = new("pixlet");

    /// <summary>
    ///     PPM (Portable PixelMap) image
    /// </summary>
    public static VideoCodec Ppm { get; } = new("ppm");

    /// <summary>
    ///     Apple ProRes (iCodec Pro)
    /// </summary>
    public static VideoCodec Prores { get; } = new("prores");

    /// <summary>
    ///     Brooktree ProSumer Video
    /// </summary>
    public static VideoCodec Prosumer { get; } = new("prosumer");

    /// <summary>
    ///     Photoshop PSD file
    /// </summary>
    public static VideoCodec Psd { get; } = new("psd");

    /// <summary>
    ///     V.Flash PTX image
    /// </summary>
    public static VideoCodec Ptx { get; } = new("ptx");

    /// <summary>
    ///     Apple QuickDraw
    /// </summary>
    public static VideoCodec Qdraw { get; } = new("qdraw");

    /// <summary>
    ///     Q-team QPEG
    /// </summary>
    public static VideoCodec Qpeg { get; } = new("qpeg");

    /// <summary>
    ///     QuickTime Animation (RLE) video
    /// </summary>
    public static VideoCodec Qtrle { get; } = new("qtrle");

    /// <summary>
    ///     AJA Kona 10-bit RGB Codec
    /// </summary>
    public static VideoCodec R10K { get; } = new("r10k");

    /// <summary>
    ///     Uncompressed RGB 10-bit
    /// </summary>
    public static VideoCodec R210 { get; } = new("r210");

    /// <summary>
    ///     RemotelyAnywhere Screen Capture
    /// </summary>
    public static VideoCodec Rasc { get; } = new("rasc");

    /// <summary>
    ///     raw video
    /// </summary>
    public static VideoCodec Rawvideo { get; } = new("rawvideo");

    /// <summary>
    ///     RL2 video
    /// </summary>
    public static VideoCodec Rl2 { get; } = new("rl2");

    /// <summary>
    ///     id RoQ video
    /// </summary>
    public static VideoCodec Roq { get; } = new("roq");

    /// <summary>
    ///     QuickTime video (RPZA)
    /// </summary>
    public static VideoCodec Rpza { get; } = new("rpza");

    /// <summary>
    ///     innoHeim/Rsupport Screen Capture Codec
    /// </summary>
    public static VideoCodec Rscc { get; } = new("rscc");

    /// <summary>
    ///     RealVideo 1.0
    /// </summary>
    public static VideoCodec Rv10 { get; } = new("rv10");

    /// <summary>
    ///     RealVideo 2.0
    /// </summary>
    public static VideoCodec Rv20 { get; } = new("rv20");

    /// <summary>
    ///     RealVideo 3.0
    /// </summary>
    public static VideoCodec Rv30 { get; } = new("rv30");

    /// <summary>
    ///     RealVideo 4.0
    /// </summary>
    public static VideoCodec Rv40 { get; } = new("rv40");

    /// <summary>
    ///     LucasArts SANM/SMUSH video
    /// </summary>
    public static VideoCodec Sanm { get; } = new("sanm");

    /// <summary>
    ///     ScreenPressor
    /// </summary>
    public static VideoCodec Scpr { get; } = new("scpr");

    /// <summary>
    ///     Screenpresso
    /// </summary>
    public static VideoCodec Screenpresso { get; } = new("screenpresso");

    /// <summary>
    ///     SGI image
    /// </summary>
    public static VideoCodec Sgi { get; } = new("sgi");

    /// <summary>
    ///     SGI RLE 8-bit
    /// </summary>
    public static VideoCodec Sgirle { get; } = new("sgirle");

    /// <summary>
    ///     BitJazz SheerVideo
    /// </summary>
    public static VideoCodec Sheervideo { get; } = new("sheervideo");

    /// <summary>
    ///     Smacker video
    /// </summary>
    public static VideoCodec Smackvideo { get; } = new("smackvideo");

    /// <summary>
    ///     QuickTime Graphics (SMC)
    /// </summary>
    public static VideoCodec Smc { get; } = new("smc");

    /// <summary>
    ///     Sigmatel Motion Video
    /// </summary>
    public static VideoCodec Smvjpeg { get; } = new("smvjpeg");

    /// <summary>
    ///     Snow
    /// </summary>
    public static VideoCodec Snow { get; } = new("snow");

    /// <summary>
    ///     Sunplus JPEG (SP5X)
    /// </summary>
    public static VideoCodec Sp5X { get; } = new("sp5x");

    /// <summary>
    ///     NewTek SpeedHQ
    /// </summary>
    public static VideoCodec Speedhq { get; } = new("speedhq");

    /// <summary>
    ///     Screen Recorder Gold Codec
    /// </summary>
    public static VideoCodec Srgc { get; } = new("srgc");

    /// <summary>
    ///     Sun Rasterfile image
    /// </summary>
    public static VideoCodec Sunrast { get; } = new("sunrast");

    /// <summary>
    ///     Scalable Vector Graphics
    /// </summary>
    public static VideoCodec Svg { get; } = new("svg");

    /// <summary>
    ///     Sorenson Vector Quantizer 1 / Sorenson Video 1 / SVQ1
    /// </summary>
    public static VideoCodec Svq1 { get; } = new("svq1");

    /// <summary>
    ///     Sorenson Vector Quantizer 3 / Sorenson Video 3 / SVQ3
    /// </summary>
    public static VideoCodec Svq3 { get; } = new("svq3");

    /// <summary>
    ///     Truevision Targa image
    /// </summary>
    public static VideoCodec Targa { get; } = new("targa");

    /// <summary>
    ///     Pinnacle TARGA CineWave YUV16
    /// </summary>
    public static VideoCodec TargaY216 { get; } = new("targa_y216");

    /// <summary>
    ///     TDSC
    /// </summary>
    public static VideoCodec Tdsc { get; } = new("tdsc");

    /// <summary>
    ///     Electronic Arts TGQ video
    /// </summary>
    public static VideoCodec Tgq { get; } = new("tgq");

    /// <summary>
    ///     Electronic Arts TGV video
    /// </summary>
    public static VideoCodec Tgv { get; } = new("tgv");

    /// <summary>
    ///     Nintendo Gamecube THP video
    /// </summary>
    public static VideoCodec Thp { get; } = new("thp");

    /// <summary>
    ///     Tiertex Limited SEQ video
    /// </summary>
    public static VideoCodec Tiertexseqvideo { get; } = new("tiertexseqvideo");

    /// <summary>
    ///     TIFF image
    /// </summary>
    public static VideoCodec Tiff { get; } = new("tiff");

    /// <summary>
    ///     8088flex TMV
    /// </summary>
    public static VideoCodec Tmv { get; } = new("tmv");

    /// <summary>
    ///     Electronic Arts TQI video
    /// </summary>
    public static VideoCodec Tqi { get; } = new("tqi");

    /// <summary>
    ///     Duck TrueMotion 1.0
    /// </summary>
    public static VideoCodec Truemotion1 { get; } = new("truemotion1");

    /// <summary>
    ///     Duck TrueMotion 2.0
    /// </summary>
    public static VideoCodec Truemotion2 { get; } = new("truemotion2");

    /// <summary>
    ///     Duck TrueMotion 2.0 Real Time
    /// </summary>
    public static VideoCodec Truemotion2Rt { get; } = new("truemotion2rt");

    /// <summary>
    ///     TechSmith Screen Capture Codec
    /// </summary>
    public static VideoCodec Tscc { get; } = new("tscc");

    /// <summary>
    ///     TechSmith Screen Codec 2
    /// </summary>
    public static VideoCodec Tscc2 { get; } = new("tscc2");

    /// <summary>
    ///     Renderware TXD (TeXture Dictionary) image
    /// </summary>
    public static VideoCodec Txd { get; } = new("txd");

    /// <summary>
    ///     IBM UltiMotion
    /// </summary>
    public static VideoCodec Ulti { get; } = new("ulti");

    /// <summary>
    ///     Ut Video
    /// </summary>
    public static VideoCodec Utvideo { get; } = new("utvideo");

    /// <summary>
    ///     Uncompressed 4:2:2 10-bit
    /// </summary>
    public static VideoCodec V210 { get; } = new("v210");

    /// <summary>
    ///     Uncompressed 4:2:2 10-bit
    /// </summary>
    public static VideoCodec V210X { get; } = new("v210x");

    /// <summary>
    ///     Uncompressed packed 4:4:4
    /// </summary>
    public static VideoCodec V308 { get; } = new("v308");

    /// <summary>
    ///     Uncompressed packed QT 4:4:4:4
    /// </summary>
    public static VideoCodec V408 { get; } = new("v408");

    /// <summary>
    ///     Uncompressed 4:4:4 10-bit
    /// </summary>
    public static VideoCodec V410 { get; } = new("v410");

    /// <summary>
    ///     Beam Software VB
    /// </summary>
    public static VideoCodec Vb { get; } = new("vb");

    /// <summary>
    ///     VBLE Lossless Codec
    /// </summary>
    public static VideoCodec Vble { get; } = new("vble");

    /// <summary>
    ///     SMPTE VC-1
    /// </summary>
    public static VideoCodec Vc1 { get; } = new("vc1");

    /// <summary>
    ///     Windows Media Video 9 Image v2
    /// </summary>
    public static VideoCodec Vc1Image { get; } = new("vc1image");

    /// <summary>
    ///     ATI VCR1
    /// </summary>
    public static VideoCodec Vcr1 { get; } = new("vcr1");

    /// <summary>
    ///     Miro VideoXL
    /// </summary>
    public static VideoCodec Vixl { get; } = new("vixl");

    /// <summary>
    ///     Sierra VMD video
    /// </summary>
    public static VideoCodec Vmdvideo { get; } = new("vmdvideo");

    /// <summary>
    ///     VMware Screen Codec / VMware Video
    /// </summary>
    public static VideoCodec Vmnc { get; } = new("vmnc");

    /// <summary>
    ///     On2 VP3
    /// </summary>
    public static VideoCodec Vp3 { get; } = new("vp3");

    /// <summary>
    ///     On2 VP4
    /// </summary>
    public static VideoCodec Vp4 { get; } = new("vp4");

    /// <summary>
    ///     On2 VP5
    /// </summary>
    public static VideoCodec Vp5 { get; } = new("vp5");

    /// <summary>
    ///     On2 VP6
    /// </summary>
    public static VideoCodec Vp6 { get; } = new("vp6");

    /// <summary>
    ///     On2 VP6 (Flash version, with alpha channel)
    /// </summary>
    public static VideoCodec Vp6A { get; } = new("vp6a");

    /// <summary>
    ///     On2 VP6 (Flash version)
    /// </summary>
    public static VideoCodec Vp6F { get; } = new("vp6f");

    /// <summary>
    ///     On2 VP7
    /// </summary>
    public static VideoCodec Vp7 { get; } = new("vp7");

    /// <summary>
    ///     Google VP9
    /// </summary>
    public static VideoCodec Vp9 { get; } = new("vp9");

    /// <summary>
    ///     WinCAM Motion Video
    /// </summary>
    public static VideoCodec Wcmv { get; } = new("wcmv");

    /// <summary>
    ///     WebP
    /// </summary>
    public static VideoCodec Webp { get; } = new("webp");

    /// <summary>
    ///     Windows Media Video 7
    /// </summary>
    public static VideoCodec Wmv1 { get; } = new("wmv1");

    /// <summary>
    ///     Windows Media Video 8
    /// </summary>
    public static VideoCodec Wmv2 { get; } = new("wmv2");

    /// <summary>
    ///     Windows Media Video 9
    /// </summary>
    public static VideoCodec Wmv3 { get; } = new("wmv3");

    /// <summary>
    ///     Windows Media Video 9 Image
    /// </summary>
    public static VideoCodec Wmv3Image { get; } = new("wmv3image");

    /// <summary>
    ///     Winnov WNV1
    /// </summary>
    public static VideoCodec Wnv1 { get; } = new("wnv1");

    /// <summary>
    ///     AVFrame to AVPacket passthrough
    /// </summary>
    public static VideoCodec WrappedAvframe { get; } = new("wrapped_avframe");

    /// <summary>
    ///     Westwood Studios VQA (Vector Quantized Animation) video
    /// </summary>
    public static VideoCodec WsVqa { get; } = new("ws_vqa");

    /// <summary>
    ///     Wing Commander III / Xan
    /// </summary>
    public static VideoCodec XanWc3 { get; } = new("xan_wc3");

    /// <summary>
    ///     Wing Commander IV / Xxan
    /// </summary>
    public static VideoCodec XanWc4 { get; } = new("xan_wc4");

    /// <summary>
    ///     eXtended BINary text
    /// </summary>
    public static VideoCodec Xbin { get; } = new("xbin");

    /// <summary>
    ///     XBM (X BitMap) image
    /// </summary>
    public static VideoCodec Xbm { get; } = new("xbm");

    /// <summary>
    ///     X-face image
    /// </summary>
    public static VideoCodec Xface { get; } = new("xface");

    /// <summary>
    ///     XPM (X PixMap) image
    /// </summary>
    public static VideoCodec Xpm { get; } = new("xpm");

    /// <summary>
    ///     XWD (X Window Dump) image
    /// </summary>
    public static VideoCodec Xwd { get; } = new("xwd");

    /// <summary>
    ///     Uncompressed YUV 4:1:1 12-bit
    /// </summary>
    public static VideoCodec Y41P { get; } = new("y41p");

    /// <summary>
    ///     YUY2 Lossless Codec
    /// </summary>
    public static VideoCodec Ylc { get; } = new("ylc");

    /// <summary>
    ///     Psygnosis YOP Video
    /// </summary>
    public static VideoCodec Yop { get; } = new("yop");

    /// <summary>
    ///     Uncompressed packed 4:2:0
    /// </summary>
    public static VideoCodec Yuv4 { get; } = new("yuv4");

    /// <summary>
    ///     ZeroCodec Lossless Video
    /// </summary>
    public static VideoCodec Zerocodec { get; } = new("zerocodec");

    /// <summary>
    ///     LCL (LossLess Codec Library) ZLIB
    /// </summary>
    public static VideoCodec Zlib { get; } = new("zlib");

    /// <summary>
    ///     Zip Motion Blocks Video
    /// </summary>
    public static VideoCodec Zmbv { get; } = new("zmbv");

    /// <summary>
    ///     Intel QuickSync Video HEVC encoder
    /// </summary>
    public static VideoCodec HevcQsv { get; } = new("hevc_qsv");
}
