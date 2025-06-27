﻿namespace Clearline.MediaFlow.Tests;

public static class FileInfoExtensions
{
    public static Task<byte[]> ReadAllBytesAsync(this FileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        return File.ReadAllBytesAsync(fileInfo.FullName, cancellationToken);
    }
}
