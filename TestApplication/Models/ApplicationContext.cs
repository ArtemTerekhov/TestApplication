using Microsoft.EntityFrameworkCore;

namespace TestApplication.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Location> Locations { get; set; }
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var firstGender = new Gender
            {
                GenderId = 1,
                Name = "Мужской",
            };

            var secondGender = new Gender
            {
                GenderId = 2,
                Name = "Женский"
            };

            var firstLocation = new Location
            {
                LocationId = 1,
                Name = "New York"
            };

            var secondLocation = new Location
            {
                LocationId = 2,
                Name = "Washington"
            };

            var firstClient = new Client
            {
                ClientId = 1,
                Name = "Neil Armstrong",
                GenderId = firstGender.GenderId,
                LocationId = firstLocation.LocationId
            };

            var secondClient = new Client
            {
                ClientId = 2,
                Name = "Buzz Aldrin",
                GenderId = firstGender.GenderId,
                LocationId = secondLocation.LocationId
            };

            var thirdClient = new Client
            {
                ClientId = 3,
                Name = "Sally Ride",
                GenderId = secondGender.GenderId,
                LocationId = firstLocation.LocationId
            };

            modelBuilder.Entity<Gender>().HasData(new Gender[] { firstGender, secondGender });
            modelBuilder.Entity<Location>().HasData(new Location[] { firstLocation,
                secondLocation });
            modelBuilder.Entity<Client>().HasData(new Client[] { firstClient, secondClient,
                thirdClient });
                                
            base.OnModelCreating(modelBuilder);
        }
    }
}
