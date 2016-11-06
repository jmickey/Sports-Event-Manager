using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SportsManager.Models
{
    public class SportsContext : DbContext
    {
        public SportsContext() : base("DefaultConnection") { }

        public DbSet<Game> Games { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EventCompetitor> EventCompetitors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Competitors).WithMany(c => c.Games)
                .Map(t => t.MapLeftKey("gameID")
                    .MapRightKey("competitorID")
                    .ToTable("GameCompetitors"));

            modelBuilder.Entity<Photo>()
                .HasMany(p => p.Tags).WithMany(t => t.Photos)
                .Map(t => t.MapLeftKey("photoID")
                    .MapRightKey("tagID")
                    .ToTable("PhotoTags"));

            // Set the primary key for the EventCompetitors table as a composite key
            modelBuilder.Entity<EventCompetitor>().HasKey(ec =>
                new { ec.eventID, ec.competitorID }
            );

            // Manually map the relationships to the EventCompetitors table
            // http://stackoverflow.com/a/30616221
            modelBuilder.Entity<EventCompetitor>()
                .HasRequired(ec => ec.Competitor)
                .WithMany(ec => ec.EventCompetitors)
                .HasForeignKey(ec => ec.competitorID);

            modelBuilder.Entity<EventCompetitor>()
                .HasRequired(ec => ec.Event)
                .WithMany(ec => ec.EventCompetitors)
                .HasForeignKey(ec => ec.eventID);
        }
    }
}