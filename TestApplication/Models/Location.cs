namespace TestApplication.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public HashSet<Client> Clients { get; set; }
    }
}
