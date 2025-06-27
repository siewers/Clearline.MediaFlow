namespace Clearline.MediaFlow.Tests.Fixtures;

public sealed class StorageFixture : IAsyncLifetime
{
    public DirectoryInfo TempDirectory { get; private set; } = null!;

    public ValueTask InitializeAsync()
    {
        TempDirectory = Directory.CreateTempSubdirectory("FFmpegDotNetTests");
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        while (TempDirectory.Exists)
        {
            try
            {
                TempDirectory.Delete(true);
                break;
            }
            catch
            {
                await Task.Delay(500.Milliseconds());
            }
        }
    }

    public MediaLocationBuilder CreateFile()
    {
        return new MediaLocationBuilder(this);
    }

    public string GetTempDirectory()
    {
        var path = Path.Combine(TempDirectory.FullName, Path.GetRandomFileName());
        Directory.CreateDirectory(path);
        return path;
    }

    public sealed class MediaLocationBuilder(StorageFixture storageFixture)
    {
        private MediaLocation? _builtMediaLocation;
        private string? _directory;
        private string? _extension;
        private string? _fileName;

        public MediaLocationBuilder WithDirectory(string directory)
        {
            _directory = directory;
            return this;
        }

        public MediaLocationBuilder WithName(string fileName)
        {
            _fileName = fileName;
            return this;
        }

        public MediaLocationBuilder WithExtension(FileExtension extension)
        {
            _extension = extension.ToString().ToLowerInvariant();
            return this;
        }

        public static implicit operator MediaLocation(MediaLocationBuilder builder)
        {
            return builder.Build();
        }

        public static implicit operator string(MediaLocationBuilder builder)
        {
            return builder.Build();
        }

        public override string ToString()
        {
            return Build();
        }

        private MediaLocation Build()
        {
            _directory ??= storageFixture.TempDirectory.FullName;
            _fileName = Path.ChangeExtension(_fileName ?? Path.GetRandomFileName(), _extension);
            var filePath = Path.Combine(_directory, _fileName);
            _builtMediaLocation ??= MediaLocation.Create(filePath);
            return _builtMediaLocation;
        }

        public long GetFileSize()
        {
            if (_builtMediaLocation == null)
            {
                throw new InvalidOperationException("MediaLocation has not been built yet.");
            }

            return new FileInfo(_builtMediaLocation).Length;
        }
    }
}
