// ReSharper disable InconsistentNaming

namespace Clearline.MediaFlow;

using NetEscapades.EnumGenerators;

[PublicAPI]
[EnumExtensions]
public enum VideoSyncMethod
{
    passthrough,
    cfr,
    vfr,
    drop,
    auto,
}
