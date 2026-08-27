using System.Text.Json.Serialization;

namespace NFCEPS.Domain.Models
{
    public class StatusResponse
    {
        [JsonIgnore]
        public int Status { get; set; }
        
        [JsonIgnore]
        public string? MSG { get; set; }
    }
}
