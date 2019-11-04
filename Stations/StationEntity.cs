using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using itec_mobile_api_final.Entities;
using itec_mobile_api_final.Sockets;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Stations
{
    [Table("Stations", Schema = "mobile-api")]
    public class StationEntity : Entity
    {
        public string Name { get; set; }
        [JsonIgnore]
        public List<SocketsEntity> Sockets { get; set; }
        [JsonIgnore]
        public string LocationStr { get; set; }
        [NotMapped]
        public PointF Location
        {
            get => JsonConvert.DeserializeObject<PointF>(LocationStr);
            set => LocationStr = JsonConvert.SerializeObject(value);
        }
    }
}