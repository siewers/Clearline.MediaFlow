// ReSharper disable InconsistentNaming

namespace Clearline.MediaFlow;

using NetEscapades.EnumGenerators;

[PublicAPI]
[EnumExtensions]
public enum AmplitudeScale
{
    lin,
    sqrt,
    cbrt,
    log,
}
