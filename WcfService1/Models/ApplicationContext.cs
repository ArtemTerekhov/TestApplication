using System.Data.Entity;

namespace WcfService1.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Location> Locations { get; set; }

        public ApplicationContext() : base("Server=DESKTOP-Q0Q2QTO\\SQLEXPRESS;Database=clients;User Id=sa;Password=KFDjG&6DfAV;")  {  }
    }
}