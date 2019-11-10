using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using itec_mobile_api_final.Stations;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Votes
{
    public class VoteEntity : Entity
    {
        public bool? Vote { get; set; }
        
        [ReadOnly(true)]
        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [ReadOnly(true)]
        public string StationId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(StationId))]
        public StationEntity Station { get; set; }
    }
}