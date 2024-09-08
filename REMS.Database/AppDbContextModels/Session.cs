namespace REMS.Database.AppDbContextModels;

public class Session
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string RefreshToken { get; set; } = null!;

    public DateTime ExpiredTime { get; set; }

    public DateTime LastActiveTime { get; set; }
}