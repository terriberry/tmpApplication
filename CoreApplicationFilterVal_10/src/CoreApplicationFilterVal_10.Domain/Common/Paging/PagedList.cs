using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Common.PagedList", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Paging;

public class PagedList<T>
{
    public int TotalCount { get; set; }
    public int PageCount { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public List<T> Items { get; set; }
}