using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
//        [JsonIgnore]
        public Point LocationStr { get; set; }
        
//        [NotMapped]
//        public Point Location
//        {
//            get => JsonConvert.DeserializeObject<Point>(LocationStr);
//            set => LocationStr = JsonConvert.SerializeObject(value);
//        }
    }
    [Serializable]
    [NotMapped]
    public class Point
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
}