namespace Clearline.MediaFlow;

using System.Collections;

public sealed class FilterCollection : IEnumerable<Filter>
{
    private static readonly IEqualityComparer<Filter> FilterComparer = EqualityComparer<Filter>.Create((left, right) => left.Name.Equals(right.Name), f => f.Name.GetHashCode());
    private readonly HashSet<Filter> _filters = new(FilterComparer);

    public int Count => _filters.Count;

    public void Add(string name, string? value)
    {
        _filters.Add(new Filter(name, value));
    }

    public void Add(Filter filter)
    {
        _filters.Add(filter);
    }

    public IEnumerator<Filter> GetEnumerator()
    {
        return _filters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_filters).GetEnumerator();
    }
}
