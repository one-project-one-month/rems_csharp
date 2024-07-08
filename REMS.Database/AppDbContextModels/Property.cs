using System;
using System.Collections.Generic;

namespace REMS.Database.AppDbContextModels;

public partial class Property
{
    public int PropertyId { get; set; }

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

    public DateTime? DateListed { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
