using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfService1.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public HashSet<Client> Clients { get; set; }
    }
}
