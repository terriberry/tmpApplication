using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Common.PagedFilter", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Paging;

public class PagedFilter
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string OrderBy { get; set; }
    public string OrderByDesc { get; set; }
}