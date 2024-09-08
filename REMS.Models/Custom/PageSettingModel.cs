namespace REMS.Models.Custom;

public class PageSettingModel
{
    public PageSettingModel(int pageNo, int pageSize, int pageCount, int totalCount)
    {
        PageNo = pageNo;
        PageSize = pageSize;
        PageCount = pageCount;
        TotalCount = totalCount;
    }

    public int TotalCount { get; set; }
    private int PageCount { get; }
    private int PageNo { get; }
    public int PageSize { get; set; }
    public bool IsEndOfPage => PageNo >= PageCount;
}