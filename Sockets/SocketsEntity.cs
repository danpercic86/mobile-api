using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using itec_mobile_api_final.Stations;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Sockets
{
    [Table("Sockets", Schema = "mobile-api")]
    public class SocketsEntity : Entity
    {
        public SocketEnum Type { get; set; }
        public StateEnum State { get; set; }
        
        [JsonIgnore]
        public StationEntity Station { get; set; }
    }
}