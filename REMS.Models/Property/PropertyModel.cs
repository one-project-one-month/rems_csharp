namespace REMS.Models.Property;

public class PropertyModel
{
    public int PropertyId { get; set; }

    public int? AgentId { get; set; }

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string PropertyType { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Size { get; set; }

    public int? NumberOfBedrooms { get; set; }

    public int? NumberOfBathrooms { get; set; }

    public int? YearBuilt { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public string AvailiablityType { get; set; } = null!;

    public int? MinrentalPeriod { get; set; }

    public string? Approvedby { get; set; }

    public DateTime? Adddate { get; set; }

    public DateTime? Editdate { get; set; }
}