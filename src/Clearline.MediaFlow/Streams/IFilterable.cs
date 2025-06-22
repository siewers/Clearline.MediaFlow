namespace Clearline.MediaFlow;

internal interface IFilterable2
{
    IEnumerable<IFilterConfiguration> GetFilters();
}
