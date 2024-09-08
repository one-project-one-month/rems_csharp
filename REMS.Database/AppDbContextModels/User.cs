namespace REMS.Database.AppDbContextModels;

public class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string Role { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public virtual ICollection<Agent> Agents { get; set; } = new List<Agent>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}