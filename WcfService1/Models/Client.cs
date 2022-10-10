
namespace WcfService1.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
