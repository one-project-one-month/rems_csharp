﻿namespace REMS.Models.Appointment;

public class AppointmentRequestModel
{
    public int? ClientId { get; set; }

    public int? PropertyId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string? AppointmentTime { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }
}