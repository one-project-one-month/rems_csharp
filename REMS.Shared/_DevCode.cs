namespace REMS.Shared;

public static class _DevCode
{
    public static IQueryable<T> Pagination<T>(this IQueryable<T> query,int pageNo,int pageSize)
    {
        return query.Skip((pageNo-1)*pageSize).Take(pageSize);
    }
}