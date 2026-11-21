using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Carriage> Carriages { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Явное соответствие таблицам и колонкам (snake_case как в ЛР9)
        modelBuilder.Entity<Carriage>(e =>
        {
            e.ToTable("carriages");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Type).HasColumnName("type");
        });

        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Login).HasColumnName("login");
            e.Property(x => x.FullName).HasColumnName("full_name");
            e.Property(x => x.Role).HasColumnName("role");
        });

        modelBuilder.Entity<Ticket>(e =>
        {
            e.ToTable("trips");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Number).HasColumnName("number");
            e.Property(x => x.Title).HasColumnName("title");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.CarriageId).HasColumnName("carriage_id");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            e.Property(x => x.AssigneeUserId).HasColumnName("assignee_user_id");
        });
    }
}