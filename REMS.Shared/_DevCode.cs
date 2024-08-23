namespace REMS.Shared;

public static class _DevCode
{
    public static IQueryable<T> Pagination<T>(this IQueryable<T> query, int pageNo, int pageSize)
    {
        return query.Skip((pageNo - 1) * pageSize).Take(pageSize);
    }

    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T? ToObject<T>(this string jsonStr)
    {
        return JsonConvert.DeserializeObject<T>(jsonStr);
    }
}