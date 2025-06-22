namespace Clearline.MediaFlow;

// ReSharper disable InconsistentNaming
public readonly partial record struct AudioCodec
{
    /// <summary>
    ///     Copy audio stream without re-encoding.
    /// </summary>
    public static AudioCodec Copy => new("copy");

    /// <summary>
    ///     8SVX fibonacci
    /// </summary>
    public static AudioCodec _8svx_fib { get; } = new("8svx_fib");

    /// <summary>
    ///     8SVX exponential
    /// </summary>
    public static AudioCodec _8svx_exp { get; } = new("8svx_exp");

    /// <summary>
    ///     4GV (Fourth Generation Vocoder)
    /// </summary>
    public static AudioCodec _4gv { get; } = new("4gv");

    /// <summary>
    ///     MP2 (MPEG audio layer 2)
    /// </summary>
    public static AudioCodec Mp2 { get; } = new("mp2");

    /// <summary>
    ///     RFC 3389 Comfort Noise
    /// </summary>
    public static AudioCodec ComfortNoise { get; } = new("comfortnoise");

    /// <summary>
    ///     AAC (Advanced Audio Coding)
    /// </summary>
    public static AudioCodec Aac { get; } = new("aac");

    /// <summary>
    ///     ATSC A/52A (AC-3)
    /// </summary>
    public static AudioCodec Ac3 { get; } = new("ac3");

    /// <summary>
    ///     Vorbis
    /// </summary>
    public static AudioCodec Libvorbis { get; } = new("libvorbis");

    /// <summary>
    ///     AAC LATM (Advanced Audio Coding LATM syntax)
    /// </summary>
    public static AudioCodec AacLatm { get; } = new("aac_latm");

    /// <summary>
    ///     ADPCM 4X Movie
    /// </summary>
    public static AudioCodec Adpcm4xm { get; } = new("adpcm_4xm");

    /// <summary>
    ///     SEGA CRI ADX ADPCM
    /// </summary>
    public static AudioCodec AdpcmAdx { get; } = new("adpcm_adx");

    /// <summary>
    ///     ADPCM Nintendo Gamecube AFC
    /// </summary>
    public static AudioCodec AdpcmAfc { get; } = new("adpcm_afc");

    /// <summary>
    ///     ADPCM AmuseGraphics Movie AGM
    /// </summary>
    public static AudioCodec AdpcmAgm { get; } = new("adpcm_agm");

    /// <summary>
    ///     ADPCM Yamaha AICA
    /// </summary>
    public static AudioCodec AdpcmAica { get; } = new("adpcm_aica");

    /// <summary>
    ///     ADPCM Creative Technology
    /// </summary>
    public static AudioCodec AdpcmCt { get; } = new("adpcm_ct");

    /// <summary>
    ///     ADPCM Nintendo Gamecube DTK
    /// </summary>
    public static AudioCodec AdpcmDtk { get; } = new("adpcm_dtk");

    /// <summary>
    ///     ADPCM Electronic Arts
    /// </summary>
    public static AudioCodec AdpcmEa { get; } = new("adpcm_ea");

    /// <summary>
    ///     ADPCM Electronic Arts Maxis CDROM XA
    /// </summary>
    public static AudioCodec AdpcmEaMaxisXa { get; } = new("adpcm_ea_maxis_xa");

    /// <summary>
    ///     ADPCM Electronic Arts R1
    /// </summary>
    public static AudioCodec AdpcmEaR1 { get; } = new("adpcm_ea_r1");

    /// <summary>
    ///     ADPCM Electronic Arts R2
    /// </summary>
    public static AudioCodec AdpcmEaR2 { get; } = new("adpcm_ea_r2");

    /// <summary>
    ///     ADPCM Electronic Arts R3
    /// </summary>
    public static AudioCodec AdpcmEaR3 { get; } = new("adpcm_ea_r3");

    /// <summary>
    ///     ADPCM Electronic Arts XAS
    /// </summary>
    public static AudioCodec AdpcmEaXas { get; } = new("adpcm_ea_xas");

    /// <summary>
    ///     G.722 ADPCM
    /// </summary>
    public static AudioCodec AdpcmG722 { get; } = new("adpcm_g722");

    /// <summary>
    ///     G.726 ADPCM
    /// </summary>
    public static AudioCodec AdpcmG726 { get; } = new("adpcm_g726");

    /// <summary>
    ///     G.726 ADPCM little-endian
    /// </summary>
    public static AudioCodec AdpcmG726le { get; } = new("adpcm_g726le");

    /// <summary>
    ///     ADPCM IMA AMV
    /// </summary>
    public static AudioCodec AdpcmImaAmv { get; } = new("adpcm_ima_amv");

    /// <summary>
    ///     ADPCM IMA CRYO APC
    /// </summary>
    public static AudioCodec AdpcmImaApc { get; } = new("adpcm_ima_apc");

    /// <summary>
    ///     ADPCM IMA Eurocom DAT4
    /// </summary>
    public static AudioCodec AdpcmImaDat4 { get; } = new("adpcm_ima_dat4");

    /// <summary>
    ///     ADPCM IMA Duck DK3
    /// </summary>
    public static AudioCodec AdpcmImaDk3 { get; } = new("adpcm_ima_dk3");

    /// <summary>
    ///     ADPCM IMA Duck DK4
    /// </summary>
    public static AudioCodec AdpcmImaDk4 { get; } = new("adpcm_ima_dk4");

    /// <summary>
    ///     ADPCM IMA Electronic Arts EACS
    /// </summary>
    public static AudioCodec AdpcmImaEaEacs { get; } = new("adpcm_ima_ea_eacs");

    /// <summary>
    ///     ADPCM IMA Electronic Arts SEAD
    /// </summary>
    public static AudioCodec AdpcmImaEaSead { get; } = new("adpcm_ima_ea_sead");

    /// <summary>
    ///     ADPCM IMA Funcom ISS
    /// </summary>
    public static AudioCodec AdpcmImaIss { get; } = new("adpcm_ima_iss");

    /// <summary>
    ///     ADPCM IMA Dialogic OKI
    /// </summary>
    public static AudioCodec AdpcmImaOki { get; } = new("adpcm_ima_oki");

    /// <summary>
    ///     ADPCM IMA QuickTime
    /// </summary>
    public static AudioCodec AdpcmImaQt { get; } = new("adpcm_ima_qt");

    /// <summary>
    ///     ADPCM IMA Radical
    /// </summary>
    public static AudioCodec AdpcmImaRad { get; } = new("adpcm_ima_rad");

    /// <summary>
    ///     ADPCM IMA Loki SDL MJPEG
    /// </summary>
    public static AudioCodec AdpcmImaSmjpeg { get; } = new("adpcm_ima_smjpeg");

    /// <summary>
    ///     ADPCM IMA WAV
    /// </summary>
    public static AudioCodec AdpcmImaWav { get; } = new("adpcm_ima_wav");

    /// <summary>
    ///     ADPCM IMA Westwood
    /// </summary>
    public static AudioCodec AdpcmImaWs { get; } = new("adpcm_ima_ws");

    /// <summary>
    ///     ADPCM Microsoft
    /// </summary>
    public static AudioCodec AdpcmMs { get; } = new("adpcm_ms");

    /// <summary>
    ///     ADPCM MTAF
    /// </summary>
    public static AudioCodec AdpcmMtaf { get; } = new("adpcm_mtaf");

    /// <summary>
    ///     ADPCM Playstation
    /// </summary>
    public static AudioCodec AdpcmPsx { get; } = new("adpcm_psx");

    /// <summary>
    ///     ADPCM Sound Blaster Pro 2-bit
    /// </summary>
    public static AudioCodec AdpcmSbpro2 { get; } = new("adpcm_sbpro_2");

    /// <summary>
    ///     ADPCM Sound Blaster Pro 2.6-bit
    /// </summary>
    public static AudioCodec AdpcmSbpro3 { get; } = new("adpcm_sbpro_3");

    /// <summary>
    ///     ADPCM Sound Blaster Pro 4-bit
    /// </summary>
    public static AudioCodec AdpcmSbpro4 { get; } = new("adpcm_sbpro_4");

    /// <summary>
    ///     ADPCM Shockwave Flash
    /// </summary>
    public static AudioCodec AdpcmSwf { get; } = new("adpcm_swf");

    /// <summary>
    ///     ADPCM Nintendo THP
    /// </summary>
    public static AudioCodec AdpcmThp { get; } = new("adpcm_thp");

    /// <summary>
    ///     ADPCM Nintendo THP (Little-Endian)
    /// </summary>
    public static AudioCodec AdpcmThpLe { get; } = new("adpcm_thp_le");

    /// <summary>
    ///     LucasArts VIMA audio
    /// </summary>
    public static AudioCodec AdpcmVima { get; } = new("adpcm_vima");

    /// <summary>
    ///     ADPCM CDROM XA
    /// </summary>
    public static AudioCodec AdpcmXa { get; } = new("adpcm_xa");

    /// <summary>
    ///     ADPCM Yamaha
    /// </summary>
    public static AudioCodec AdpcmYamaha { get; } = new("adpcm_yamaha");

    /// <summary>
    ///     ALAC (Apple Lossless Audio Codec)
    /// </summary>
    public static AudioCodec Alac { get; } = new("alac");

    /// <summary>
    ///     AMR-NB (Adaptive Multi-Rate NarrowBand)
    /// </summary>
    public static AudioCodec AmrNb { get; } = new("amr_nb");

    /// <summary>
    ///     AMR-WB (Adaptive Multi-Rate WideBand)
    /// </summary>
    public static AudioCodec AmrWb { get; } = new("amr_wb");

    /// <summary>
    ///     Monkey's Audio
    /// </summary>
    public static AudioCodec Ape { get; } = new("ape");

    /// <summary>
    ///     aptX (Audio Processing Technology for Bluetooth)
    /// </summary>
    public static AudioCodec Aptx { get; } = new("aptx");

    /// <summary>
    ///     aptX HD (Audio Processing Technology for Bluetooth)
    /// </summary>
    public static AudioCodec AptxHd { get; } = new("aptx_hd");

    /// <summary>
    ///     ATRAC1 (Adaptive TRansform Acoustic Coding)
    /// </summary>
    public static AudioCodec Atrac1 { get; } = new("atrac1");

    /// <summary>
    ///     ATRAC3 (Adaptive TRansform Acoustic Coding 3)
    /// </summary>
    public static AudioCodec Atrac3 { get; } = new("atrac3");

    /// <summary>
    ///     ATRAC3 AL (Adaptive TRansform Acoustic Coding 3 Advanced Lossless)
    /// </summary>
    public static AudioCodec Atrac3al { get; } = new("atrac3al");

    /// <summary>
    ///     ATRAC3+ (Adaptive TRansform Acoustic Coding 3+)
    /// </summary>
    public static AudioCodec Atrac3p { get; } = new("atrac3p");

    /// <summary>
    ///     ATRAC3+ AL (Adaptive TRansform Acoustic Coding 3+ Advanced Lossless)
    /// </summary>
    public static AudioCodec Atrac3pal { get; } = new("atrac3pal");

    /// <summary>
    ///     ATRAC9 (Adaptive TRansform Acoustic Coding 9)
    /// </summary>
    public static AudioCodec Atrac9 { get; } = new("atrac9");

    /// <summary>
    ///     On2 Audio for Video Codec
    /// </summary>
    public static AudioCodec Avc { get; } = new("avc");

    /// <summary>
    ///     Bink Audio (DCT)
    /// </summary>
    public static AudioCodec BinkaudioDct { get; } = new("binkaudio_dct");

    /// <summary>
    ///     Bink Audio (RDFT)
    /// </summary>
    public static AudioCodec BinkaudioRdft { get; } = new("binkaudio_rdft");

    /// <summary>
    ///     Discworld II BMV audio
    /// </summary>
    public static AudioCodec BmvAudio { get; } = new("bmv_audio");

    /// <summary>
    ///     Constrained Energy Lapped Transform (CELT)
    /// </summary>
    public static AudioCodec Celt { get; } = new("celt");

    /// <summary>
    ///     codec2 (very low bitrate speech codec)
    /// </summary>
    public static AudioCodec Codec2 { get; } = new("codec2");

    /// <summary>
    ///     Cook / Cooker / Gecko (RealAudio G2)
    /// </summary>
    public static AudioCodec Cook { get; } = new("cook");

    /// <summary>
    ///     Dolby E
    /// </summary>
    public static AudioCodec DolbyE { get; } = new("dolby_e");

    /// <summary>
    ///     DSD (Direct Stream Digital), least significant bit first
    /// </summary>
    public static AudioCodec DsdLsbf { get; } = new("dsd_lsbf");

    /// <summary>
    ///     DSD (Direct Stream Digital), least significant bit first, planar
    /// </summary>
    public static AudioCodec DsdLsbfPlanar { get; } = new("dsd_lsbf_planar");

    /// <summary>
    ///     DSD (Direct Stream Digital), most significant bit first
    /// </summary>
    public static AudioCodec DsdMsbf { get; } = new("dsd_msbf");

    /// <summary>
    ///     DSD (Direct Stream Digital), most significant bit first, planar
    /// </summary>
    public static AudioCodec DsdMsbfPlanar { get; } = new("dsd_msbf_planar");

    /// <summary>
    ///     Delphine Software International CIN audio
    /// </summary>
    public static AudioCodec Dsicinaudio { get; } = new("dsicinaudio");

    /// <summary>
    ///     Digital Speech Standard - Standard Play mode (DSS SP)
    /// </summary>
    public static AudioCodec DssSp { get; } = new("dss_sp");

    /// <summary>
    ///     DST (Direct Stream Transfer)
    /// </summary>
    public static AudioCodec Dst { get; } = new("dst");

    /// <summary>
    ///     DCA (DTS Coherent Acoustics)
    /// </summary>
    public static AudioCodec Dts { get; } = new("dts");

    /// <summary>
    ///     DV audio
    /// </summary>
    public static AudioCodec Dvaudio { get; } = new("dvaudio");

    /// <summary>
    ///     ATSC A/52B (AC-3, E-AC-3)
    /// </summary>
    public static AudioCodec Eac3 { get; } = new("eac3");

    /// <summary>
    ///     EVRC (Enhanced Variable Rate Codec)
    /// </summary>
    public static AudioCodec Evrc { get; } = new("evrc");

    /// <summary>
    ///     FLAC (Free Lossless Audio Codec)
    /// </summary>
    public static AudioCodec Flac { get; } = new("flac");

    /// <summary>
    ///     G.723.1
    /// </summary>
    public static AudioCodec G723_1 { get; } = new("g723_1");

    /// <summary>
    ///     G.729
    /// </summary>
    public static AudioCodec G729 { get; } = new("g729");

    /// <summary>
    ///     DPCM Gremlin
    /// </summary>
    public static AudioCodec GremlinDpcm { get; } = new("gremlin_dpcm");

    /// <summary>
    ///     GSM
    /// </summary>
    public static AudioCodec Gsm { get; } = new("gsm");

    /// <summary>
    ///     GSM Microsoft variant
    /// </summary>
    public static AudioCodec GsmMs { get; } = new("gsm_ms");

    /// <summary>
    ///     HCOM Audio
    /// </summary>
    public static AudioCodec Hcom { get; } = new("hcom");

    /// <summary>
    ///     IAC (Indeo Audio Coder)
    /// </summary>
    public static AudioCodec Iac { get; } = new("iac");

    /// <summary>
    ///     iLBC (Internet Low Bitrate Codec)
    /// </summary>
    public static AudioCodec Ilbc { get; } = new("ilbc");

    /// <summary>
    ///     IMC (Intel Music Coder)
    /// </summary>
    public static AudioCodec Imc { get; } = new("imc");

    /// <summary>
    ///     DPCM Interplay
    /// </summary>
    public static AudioCodec InterplayDpcm { get; } = new("interplay_dpcm");

    /// <summary>
    ///     Interplay ACM
    /// </summary>
    public static AudioCodec Interplayacm { get; } = new("interplayacm");

    /// <summary>
    ///     MACE (Macintosh Audio Compression/Expansion) 3:1
    /// </summary>
    public static AudioCodec Mace3 { get; } = new("mace3");

    /// <summary>
    ///     MACE (Macintosh Audio Compression/Expansion) 6:1
    /// </summary>
    public static AudioCodec Mace6 { get; } = new("mace6");

    /// <summary>
    ///     Voxware MetaSound
    /// </summary>
    public static AudioCodec Metasound { get; } = new("metasound");

    /// <summary>
    ///     MLP (Meridian Lossless Packing)
    /// </summary>
    public static AudioCodec Mlp { get; } = new("mlp");

    /// <summary>
    ///     MP1 (MPEG audio layer 1)
    /// </summary>
    public static AudioCodec Mp1 { get; } = new("mp1");

    /// <summary>
    ///     MP3 (MPEG audio layer 3)
    /// </summary>
    public static AudioCodec Mp3 { get; } = new("mp3");

    /// <summary>
    ///     ADU (Application Data Unit) MP3 (MPEG audio layer 3)
    /// </summary>
    public static AudioCodec Mp3adu { get; } = new("mp3adu");

    /// <summary>
    ///     MP3onMP4
    /// </summary>
    public static AudioCodec Mp3on4 { get; } = new("mp3on4");

    /// <summary>
    ///     MPEG-4 Audio Lossless Coding (ALS)
    /// </summary>
    public static AudioCodec Mp4als { get; } = new("mp4als");

    /// <summary>
    ///     Musepack SV7
    /// </summary>
    public static AudioCodec Musepack7 { get; } = new("musepack7");

    /// <summary>
    ///     Musepack SV8
    /// </summary>
    public static AudioCodec Musepack8 { get; } = new("musepack8");

    /// <summary>
    ///     Nellymoser Asao
    /// </summary>
    public static AudioCodec Nellymoser { get; } = new("nellymoser");

    /// <summary>
    ///     Opus (Opus Interactive Audio Codec)
    /// </summary>
    public static AudioCodec Opus { get; } = new("opus");

    /// <summary>
    ///     Amazing Studio Packed Animation File Audio
    /// </summary>
    public static AudioCodec PafAudio { get; } = new("paf_audio");

    /// <summary>
    ///     PCM A-law / G.711 A-law
    /// </summary>
    public static AudioCodec PcmAlaw { get; } = new("pcm_alaw");

    /// <summary>
    ///     PCM signed 16|20|24-bit big-endian for Blu-ray media
    /// </summary>
    public static AudioCodec PcmBluray { get; } = new("pcm_bluray");

    /// <summary>
    ///     PCM signed 20|24-bit big-endian
    /// </summary>
    public static AudioCodec PcmDvd { get; } = new("pcm_dvd");

    /// <summary>
    ///     PCM 16.8 floating point little-endian
    /// </summary>
    public static AudioCodec PcmF16le { get; } = new("pcm_f16le");

    /// <summary>
    ///     PCM 24.0 floating point little-endian
    /// </summary>
    public static AudioCodec PcmF24le { get; } = new("pcm_f24le");

    /// <summary>
    ///     PCM 32-bit floating point big-endian
    /// </summary>
    public static AudioCodec PcmF32be { get; } = new("pcm_f32be");

    /// <summary>
    ///     PCM 32-bit floating point little-endian
    /// </summary>
    public static AudioCodec PcmF32le { get; } = new("pcm_f32le");

    /// <summary>
    ///     PCM 64-bit floating point big-endian
    /// </summary>
    public static AudioCodec PcmF64be { get; } = new("pcm_f64be");

    /// <summary>
    ///     PCM 64-bit floating point little-endian
    /// </summary>
    public static AudioCodec PcmF64le { get; } = new("pcm_f64le");

    /// <summary>
    ///     PCM signed 20-bit little-endian planar
    /// </summary>
    public static AudioCodec PcmLxf { get; } = new("pcm_lxf");

    /// <summary>
    ///     PCM mu-law / G.711 mu-law
    /// </summary>
    public static AudioCodec PcmMulaw { get; } = new("pcm_mulaw");

    /// <summary>
    ///     PCM signed 16-bit big-endian
    /// </summary>
    public static AudioCodec PcmS16be { get; } = new("pcm_s16be");

    /// <summary>
    ///     PCM signed 16-bit big-endian planar
    /// </summary>
    public static AudioCodec PcmS16bePlanar { get; } = new("pcm_s16be_planar");

    /// <summary>
    ///     PCM signed 16-bit little-endian
    /// </summary>
    public static AudioCodec PcmS16le { get; } = new("pcm_s16le");

    /// <summary>
    ///     PCM signed 16-bit little-endian planar
    /// </summary>
    public static AudioCodec PcmS16lePlanar { get; } = new("pcm_s16le_planar");

    /// <summary>
    ///     PCM signed 24-bit big-endian
    /// </summary>
    public static AudioCodec PcmS24be { get; } = new("pcm_s24be");

    /// <summary>
    ///     PCM D-Cinema audio signed 24-bit
    /// </summary>
    public static AudioCodec PcmS24daud { get; } = new("pcm_s24daud");

    /// <summary>
    ///     PCM signed 24-bit little-endian
    /// </summary>
    public static AudioCodec PcmS24le { get; } = new("pcm_s24le");

    /// <summary>
    ///     PCM signed 24-bit little-endian planar
    /// </summary>
    public static AudioCodec PcmS24lePlanar { get; } = new("pcm_s24le_planar");

    /// <summary>
    ///     PCM signed 32-bit big-endian
    /// </summary>
    public static AudioCodec PcmS32be { get; } = new("pcm_s32be");

    /// <summary>
    ///     PCM signed 32-bit little-endian
    /// </summary>
    public static AudioCodec PcmS32le { get; } = new("pcm_s32le");

    /// <summary>
    ///     PCM signed 32-bit little-endian planar
    /// </summary>
    public static AudioCodec PcmS32lePlanar { get; } = new("pcm_s32le_planar");

    /// <summary>
    ///     PCM signed 64-bit big-endian
    /// </summary>
    public static AudioCodec PcmS64be { get; } = new("pcm_s64be");

    /// <summary>
    ///     PCM signed 64-bit little-endian
    /// </summary>
    public static AudioCodec PcmS64le { get; } = new("pcm_s64le");

    /// <summary>
    ///     PCM signed 8-bit
    /// </summary>
    public static AudioCodec PcmS8 { get; } = new("pcm_s8");

    /// <summary>
    ///     PCM signed 8-bit planar
    /// </summary>
    public static AudioCodec PcmS8Planar { get; } = new("pcm_s8_planar");

    /// <summary>
    ///     PCM unsigned 16-bit big-endian
    /// </summary>
    public static AudioCodec PcmU16be { get; } = new("pcm_u16be");

    /// <summary>
    ///     PCM unsigned 16-bit little-endian
    /// </summary>
    public static AudioCodec PcmU16le { get; } = new("pcm_u16le");

    /// <summary>
    ///     PCM unsigned 24-bit big-endian
    /// </summary>
    public static AudioCodec PcmU24be { get; } = new("pcm_u24be");

    /// <summary>
    ///     PCM unsigned 24-bit little-endian
    /// </summary>
    public static AudioCodec PcmU24le { get; } = new("pcm_u24le");

    /// <summary>
    ///     PCM unsigned 32-bit big-endian
    /// </summary>
    public static AudioCodec PcmU32be { get; } = new("pcm_u32be");

    /// <summary>
    ///     PCM unsigned 32-bit little-endian
    /// </summary>
    public static AudioCodec PcmU32le { get; } = new("pcm_u32le");

    /// <summary>
    ///     PCM unsigned 8-bit
    /// </summary>
    public static AudioCodec PcmU8 { get; } = new("pcm_u8");

    /// <summary>
    ///     PCM Archimedes VIDC
    /// </summary>
    public static AudioCodec PcmVidc { get; } = new("pcm_vidc");

    /// <summary>
    ///     PCM Zork
    /// </summary>
    public static AudioCodec PcmZork { get; } = new("pcm_zork");

    /// <summary>
    ///     QCELP / PureVoice
    /// </summary>
    public static AudioCodec Qcelp { get; } = new("qcelp");

    /// <summary>
    ///     QDesign Music Codec 2
    /// </summary>
    public static AudioCodec Qdm2 { get; } = new("qdm2");

    /// <summary>
    ///     QDesign Music
    /// </summary>
    public static AudioCodec Qdmc { get; } = new("qdmc");

    /// <summary>
    ///     RealAudio 1.0 (14.4K)
    /// </summary>
    public static AudioCodec Ra144 { get; } = new("ra_144");

    /// <summary>
    ///     RealAudio 2.0 (28.8K)
    /// </summary>
    public static AudioCodec Ra288 { get; } = new("ra_288");

    /// <summary>
    ///     RealAudio Lossless
    /// </summary>
    public static AudioCodec Ralf { get; } = new("ralf");

    /// <summary>
    ///     DPCM id RoQ
    /// </summary>
    public static AudioCodec RoqDpcm { get; } = new("roq_dpcm");

    /// <summary>
    ///     SMPTE 302M
    /// </summary>
    public static AudioCodec S302m { get; } = new("s302m");

    /// <summary>
    ///     SBC (low-complexity subband codec)
    /// </summary>
    public static AudioCodec Sbc { get; } = new("sbc");

    /// <summary>
    ///     DPCM Squareroot-Delta-Exact
    /// </summary>
    public static AudioCodec Sdx2Dpcm { get; } = new("sdx2_dpcm");

    /// <summary>
    ///     Shorten
    /// </summary>
    public static AudioCodec Shorten { get; } = new("shorten");

    /// <summary>
    ///     RealAudio SIPR / ACELP.NET
    /// </summary>
    public static AudioCodec Sipr { get; } = new("sipr");

    /// <summary>
    ///     Smacker audio
    /// </summary>
    public static AudioCodec Smackaudio { get; } = new("smackaudio");

    /// <summary>
    ///     SMV (Selectable Mode Vocoder)
    /// </summary>
    public static AudioCodec Smv { get; } = new("smv");

    /// <summary>
    ///     DPCM Sol
    /// </summary>
    public static AudioCodec SolDpcm { get; } = new("sol_dpcm");

    /// <summary>
    ///     Sonic
    /// </summary>
    public static AudioCodec Sonic { get; } = new("sonic");

    /// <summary>
    ///     Sonic lossless
    /// </summary>
    public static AudioCodec Sonicls { get; } = new("sonicls");

    /// <summary>
    ///     Speex
    /// </summary>
    public static AudioCodec Speex { get; } = new("speex");

    /// <summary>
    ///     TAK (Tom's lossless Audio Kompressor)
    /// </summary>
    public static AudioCodec Tak { get; } = new("tak");

    /// <summary>
    ///     TrueHD
    /// </summary>
    public static AudioCodec Truehd { get; } = new("truehd");

    /// <summary>
    ///     DSP Group TrueSpeech
    /// </summary>
    public static AudioCodec Truespeech { get; } = new("truespeech");

    /// <summary>
    ///     TTA (True Audio)
    /// </summary>
    public static AudioCodec Tta { get; } = new("tta");

    /// <summary>
    ///     VQF TwinVQ
    /// </summary>
    public static AudioCodec Twinvq { get; } = new("twinvq");

    /// <summary>
    ///     Sierra VMD audio
    /// </summary>
    public static AudioCodec Vmdaudio { get; } = new("vmdaudio");

    /// <summary>
    ///     Vorbis
    /// </summary>
    public static AudioCodec Vorbis { get; } = new("vorbis");

    /// <summary>
    ///     Wave synthesis pseudo-codec
    /// </summary>
    public static AudioCodec Wavesynth { get; } = new("wavesynth");

    /// <summary>
    ///     WavPack
    /// </summary>
    public static AudioCodec Wavpack { get; } = new("wavpack");

    /// <summary>
    ///     Westwood Audio (SND1)
    /// </summary>
    public static AudioCodec WestwoodSnd1 { get; } = new("westwood_snd1");

    /// <summary>
    ///     Windows Media Audio Lossless
    /// </summary>
    public static AudioCodec Wmalossless { get; } = new("wmalossless");

    /// <summary>
    ///     Windows Media Audio 9 Professional
    /// </summary>
    public static AudioCodec Wmapro { get; } = new("wmapro");

    /// <summary>
    ///     Windows Media Audio 1
    /// </summary>
    public static AudioCodec Wmav1 { get; } = new("wmav1");

    /// <summary>
    ///     Windows Media Audio 2
    /// </summary>
    public static AudioCodec Wmav2 { get; } = new("wmav2");

    /// <summary>
    ///     Windows Media Audio Voice
    /// </summary>
    public static AudioCodec Wmavoice { get; } = new("wmavoice");

    /// <summary>
    ///     DPCM Xan
    /// </summary>
    public static AudioCodec XanDpcm { get; } = new("xan_dpcm");

    /// <summary>
    ///     Xbox Media Audio 1
    /// </summary>
    public static AudioCodec Xma1 { get; } = new("xma1");

    /// <summary>
    ///     Xbox Media Audio 2
    /// </summary>
    public static AudioCodec Xma2 { get; } = new("xma2");

    /// <summary>
    ///     Opus (Opus Interactive Audio Codec)
    /// </summary>
    public static AudioCodec Libopus { get; } = new("libopus");
}
