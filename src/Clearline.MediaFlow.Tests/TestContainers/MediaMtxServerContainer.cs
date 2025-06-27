﻿namespace Clearline.MediaFlow.Tests.TestContainers;

using DotNet.Testcontainers.Containers;

public sealed class MediaMtxServerContainer(MediaMtxServerConfiguration configuration)
    : DockerContainer(configuration)
{
    public Uri GetResourceUri(string path)
    {
        return new Uri(new Uri($"rtsp://{Hostname}:{GetMappedPublicPort(8554)}"), path);
    }
}
