using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using itec_mobile_api_final.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Stations
{
    public class StationEntity : Entity
    {
        public string Name { get; set; }
        public int TotalSockets { get; set; }
        public int FreeSockets { get; set; }
        public PointF Location { get; set; }
        
        [JsonIgnore]
        public bool Old { get; set; }
        
        [ReadOnly(true)]
        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        
        [ReadOnly(true)]
        public string OldStationId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(OldStationId))]
        public StationEntity OldStation { get; set; }
    }
}