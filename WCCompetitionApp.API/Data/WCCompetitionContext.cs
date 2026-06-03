using Microsoft.EntityFrameworkCore;
using WCCompetitionApp.API.Models;

namespace WCCompetitionApp.API.Data
{
    public class WCCompetitionContext : DbContext
    {
        public WCCompetitionContext(DbContextOptions<WCCompetitionContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .HasOne(ri => ri.Team1)
                .WithMany(u => u.Matches)
                .HasForeignKey(ri => ri.Team1Id);

            modelBuilder.Entity<Match>()
                .HasOne(ri => ri.Team2)
                .WithMany(u => u.Matches)
                .HasForeignKey(ri => ri.Team2Id);

            modelBuilder.Entity<GroupPlay>()
                .HasOne(ri => ri.Team1)
                .WithMany(u => u.GroupPlays)
                .HasForeignKey(ri => ri.Team1Id);

            modelBuilder.Entity<GroupPlay>()
                .HasOne(ri => ri.Team2)
                .WithMany(u => u.GroupPlays)
                .HasForeignKey(ri => ri.Team2Id);

            modelBuilder.Entity<GroupPlay>()
                .HasOne(ri => ri.Team3)
                .WithMany(u => u.GroupPlays)
                .HasForeignKey(ri => ri.Team3Id);

            modelBuilder.Entity<GroupPlay>()
                .HasOne(ri => ri.Team4)
                .WithMany(u => u.GroupPlays)
                .HasForeignKey(ri => ri.Team4Id);

            // Make fields unique
            modelBuilder.Entity<Team>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<GroupPlayGet> GrouPlays { get; set; }
    }
}
