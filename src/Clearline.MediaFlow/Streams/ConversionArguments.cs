namespace Clearline.MediaFlow;

using System.Collections;
using System.Text;

public sealed class ConversionArguments : IEnumerable<ConversionArgument>
{
    private readonly HashSet<ConversionArgument> _arguments = [];

    public IEnumerator<ConversionArgument> GetEnumerator()
    {
        return _arguments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(ConversionArgument item)
    {
        _arguments.Remove(item);
        _arguments.Add(item);
    }

    public IEnumerable<ConversionArgument> Get(ArgumentPosition position)
    {
        return _arguments.Where(argument => argument.Position == position);
    }

    public ConversionArguments AddPostInput(string name)
    {
        Add(ConversionArgument.PostInput(name));
        return this;
    }

    public ConversionArguments AddPostInput<T>(string name, T value)
    {
        Add(ConversionArgument.PostInput(name, value));
        return this;
    }

    public ConversionArguments AddPreInput(string name)
    {
        Add(ConversionArgument.PreInput(name));
        return this;
    }

    public ConversionArguments AddPreInput<T>(string name, T value)
    {
        Add(ConversionArgument.PreInput(name, value));
        return this;
    }

    public ConversionArguments Remove(string name)
    {
        _arguments.RemoveWhere(parameter => parameter.Name == name);
        return this;
    }

    public void Append(StringBuilder builder, ArgumentPosition position)
    {
        foreach (var argument in Get(position))
        {
            builder.Append($" {argument.Name} {argument.Value}".Trim());
        }
    }
}
