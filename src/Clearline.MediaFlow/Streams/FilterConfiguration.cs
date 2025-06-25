namespace Clearline.MediaFlow;

using System.Text;

internal sealed record FilterConfiguration(int StreamNumber, string FilterType, FilterCollection Filters) : IFilterConfiguration
{
    public void Append(StringBuilder builder)
    {
        foreach (var filter in Filters)
        {
            builder.Append($"[{StreamNumber}] {filter};");
        }
    }
}
