using System;
using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using itec_mobile_api_final.Sockets;

namespace itec_mobile_api_final.Cars
{
    [Table("Cars", Schema = "mobile-api")]
    public class CarEntity : Entity
    {
        public string Model { get; set; }
        public string Company { get; set; }
        public DateTime Year { get; set; }
        public float Autonomy { get; set; }
        public float BatteryLeft { get; set; }
        public SocketsEntity Socket { get; set; }
        public DateTime LastTechRevision { get; set; }
    }
}