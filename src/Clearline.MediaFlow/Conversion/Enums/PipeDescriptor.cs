// ReSharper disable InconsistentNaming

namespace Clearline.MediaFlow;

using NetEscapades.EnumGenerators;

[PublicAPI]
[EnumExtensions]
public enum PipeDescriptor
{
    stdin,
    stdout,
    stderr,
}
