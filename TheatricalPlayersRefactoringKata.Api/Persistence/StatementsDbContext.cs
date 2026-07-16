using Microsoft.EntityFrameworkCore;
using TheatricalPlayersRefactoringKata.Api.Persistence.Entities;

namespace TheatricalPlayersRefactoringKata.Api.Persistence;

public sealed class StatementsDbContext(DbContextOptions<StatementsDbContext> options)
    : DbContext(options)
{
    public DbSet<StoredStatement> Statements => Set<StoredStatement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var statement = modelBuilder.Entity<StoredStatement>();
        statement.HasKey(entity => entity.Id);
        statement.Property(entity => entity.Customer).IsRequired();
        statement.Property(entity => entity.Format).IsRequired();
        statement.Property(entity => entity.Content).IsRequired();

        statement.HasMany(entity => entity.Plays)
            .WithOne()
            .HasForeignKey(entity => entity.StatementId)
            .OnDelete(DeleteBehavior.Cascade);

        statement.HasMany(entity => entity.Performances)
            .WithOne()
            .HasForeignKey(entity => entity.StatementId)
            .OnDelete(DeleteBehavior.Cascade);

        var play = modelBuilder.Entity<StoredPlay>();
        play.HasKey(entity => entity.Id);
        play.Property(entity => entity.PlayId).IsRequired();
        play.Property(entity => entity.Name).IsRequired();
        play.Property(entity => entity.Type).IsRequired();

        var performance = modelBuilder.Entity<StoredPerformance>();
        performance.HasKey(entity => entity.Id);
        performance.Property(entity => entity.PlayId).IsRequired();
    }
}
