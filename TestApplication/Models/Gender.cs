namespace TestApplication.Models
{
    public class Gender
    {
        public int GenderId { get; set; }
        public string Name { get; set; }
        public HashSet<Client> Clients { get; set; }
    }
}
