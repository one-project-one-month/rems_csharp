using System.Security.Cryptography;

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

    public static T CheckEntityItem<T>(this object obj)
    {
        var res = default(T);
        if (obj != null && !string.IsNullOrEmpty(obj.ToString()) && obj is string)
            res = (T)Convert.ChangeType(obj.ToString().Trim(), typeof(T));
        else if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            res = (T)Convert.ChangeType(obj, typeof(T));
        return res;
    }

    private static TimeZoneInfo GetMyanmarTimeZoneInfo()
    {
        return TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time");
    }

    public static DateTime GetServerDateTime()
    {
        var timeUtc = DateTime.UtcNow;
        return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, GetMyanmarTimeZoneInfo());
    }

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public static bool IsNullOrEmpty(this object? str)
    {
        var result = true;

        result = str == null ||
                 string.IsNullOrEmpty(str.ToString()?.Trim()) ||
                 string.IsNullOrWhiteSpace(str.ToString()?.Trim());
        return result;
    }
}