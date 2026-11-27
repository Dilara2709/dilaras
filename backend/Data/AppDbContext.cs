using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Таблицы, которые ты видишь в C#-коде
    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<Carriage> Carriages => Set<Carriage>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Главный объект
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.ToTable("trips");                 // имя таблицы в PostgreSQL
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Number).HasColumnName("number").IsRequired();
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(80).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500).IsRequired();
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).IsRequired();
            entity.Property(e => e.CarriageId).HasColumnName("carriage_id");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.AssigneeUserId).HasColumnName("assignee_user_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");

            // связи
            entity.HasOne<Carriage>()
                  .WithMany()
                  .HasForeignKey(e => e.CarriageId);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(e => e.CreatedByUserId);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(e => e.AssigneeUserId)
                  .IsRequired(false);            // исполнитель может быть не назначен
        });

        modelBuilder.Entity<Carriage>(entity =>
        {
            entity.ToTable("carriages");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Type).HasColumnName("type").HasMaxLength(20).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Login).HasColumnName("login").HasMaxLength(50).IsRequired();
            entity.Property(e => e.FullName).HasColumnName("full_name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Role).HasColumnName("role").HasMaxLength(20).IsRequired();
        });
    }
}