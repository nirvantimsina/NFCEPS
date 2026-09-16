using System.Text.Json.Serialization;

namespace NFCEPS.Domain.Models
{
    public class StatusResponse
    {
        [JsonIgnore]
        public string? Status { get; set; }
        
        [JsonIgnore]
        public string? MSG { get; set; }
    }
}
