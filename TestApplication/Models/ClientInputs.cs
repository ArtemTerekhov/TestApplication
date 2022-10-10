using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestApplication.Models
{
    public class ClientInputs
    {
        [JsonPropertyName("clientId")]
        public int? ClientId { get; set; }
        [DefaultValue("Иван Иванов")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = 
            "Длина имени должна быть не менее 10 символов")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [DefaultValue("1")]
        [JsonPropertyName("genderId")]
        public int GenderId { get; set; }
        [DefaultValue("1")]
        [JsonPropertyName("locationId")]
        public int LocationId { get; set; }
    }
}
