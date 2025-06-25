namespace Clearline.MediaFlow;

public readonly record struct Filter(string Name, string? Value)
{
    public override string ToString()
    {
        return Name + (string.IsNullOrWhiteSpace(Value) ? "" : $"={Value}");
    }
}
