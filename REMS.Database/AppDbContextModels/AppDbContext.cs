using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace REMS.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agent> Agents { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Listing> Listings { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<PropertyImage> PropertyImages { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }
    s
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(e => e.AgentId).HasName("PK__Agents__2C05379E98D2C6C2");

            entity.Property(e => e.AgentId).HasColumnName("agent_id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.AgencyName)
                .HasMaxLength(100)
                .HasColumnName("agency_name");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.LicenseNumber)
                .HasMaxLength(50)
                .HasColumnName("license_number");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Agents)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Agents__user_id__3E52440B");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__A50828FCD37D2255");

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AgentId).HasColumnName("agent_id");
            entity.Property(e => e.AppointmentDate)
                .HasColumnType("date")
                .HasColumnName("appointment_date");
            entity.Property(e => e.AppointmentTime).HasColumnName("appointment_time");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Agent).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("FK__Appointme__agent__5070F446");

            entity.HasOne(d => d.Client).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Appointme__clien__5165187F");

            entity.HasOne(d => d.Property).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PropertyId)
                .HasConstraintName("FK__Appointme__prope__52593CB8");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Clients__BF21A42448228EC3");

            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.AgentId).HasColumnName("agent_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Agent).WithMany(p => p.Clients)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("FK__Clients__agent_i__4222D4EF");

            entity.HasOne(d => d.User).WithMany(p => p.Clients)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Clients__user_id__412EB0B6");
        });

        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(e => e.ListingId).HasName("PK__Listings__89D81774CBA09D57");

            entity.Property(e => e.ListingId).HasColumnName("listing_id");
            entity.Property(e => e.AgentId).HasColumnName("agent_id");
            entity.Property(e => e.DateListed)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_listed");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ListingPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("listing_price");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Agent).WithMany(p => p.Listings)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("FK__Listings__agent___45F365D3");

            entity.HasOne(d => d.Property).WithMany(p => p.Listings)
                .HasForeignKey(d => d.PropertyId)
                .HasConstraintName("FK__Listings__proper__44FF419A");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.PropertyId).HasName("PK__Properti__735BA463B88813D0");

            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.DateListed)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_listed");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.NumberOfBathrooms).HasColumnName("number_of_bathrooms");
            entity.Property(e => e.NumberOfBedrooms).HasColumnName("number_of_bedrooms");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");
            entity.Property(e => e.PropertyType)
                .HasMaxLength(50)
                .HasColumnName("property_type");
            entity.Property(e => e.Size)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("size");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.YearBuilt).HasColumnName("year_built");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .HasColumnName("zip_code");
        });

        modelBuilder.Entity<PropertyImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Property__DC9AC9551707A4DC");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.DateUploaded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_uploaded");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .HasColumnName("image_url");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");

            entity.HasOne(d => d.Property).WithMany(p => p.PropertyImages)
                .HasForeignKey(d => d.PropertyId)
                .HasConstraintName("FK__PropertyI__prope__5535A963");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__60883D9072575BD7");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.Comments).HasColumnName("comments");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Property).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.PropertyId)
                .HasConstraintName("FK__Reviews__propert__59FA5E80");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__user_id__59063A47");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__85C600AF8B6E7A59");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.AgentId).HasColumnName("agent_id");
            entity.Property(e => e.BuyerId).HasColumnName("buyer_id");
            entity.Property(e => e.Commission)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("commission");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.SalePrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("sale_price");
            entity.Property(e => e.SellerId).HasColumnName("seller_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("transaction_date");

            entity.HasOne(d => d.Agent).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("FK__Transacti__agent__4CA06362");

            entity.HasOne(d => d.Buyer).WithMany(p => p.TransactionBuyers)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK__Transacti__buyer__4AB81AF0");

            entity.HasOne(d => d.Property).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PropertyId)
                .HasConstraintName("FK__Transacti__prope__49C3F6B7");

            entity.HasOne(d => d.Seller).WithMany(p => p.TransactionSellers)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("FK__Transacti__selle__4BAC3F29");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FF1E71EBE");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164A21008DA").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
