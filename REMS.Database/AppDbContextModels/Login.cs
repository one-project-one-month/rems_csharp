using System;
using System.Collections.Generic;

namespace REMS.Database.AppDbContextModels;

public partial class Login
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string AccessToken { get; set; } = null!;

    public DateTime LoginDate { get; set; }

    public string Email { get; set; } = null!;

    public DateTime? LogoutDate { get; set; }
}
