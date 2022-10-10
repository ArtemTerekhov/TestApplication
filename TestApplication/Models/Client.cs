namespace TestApplication.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
