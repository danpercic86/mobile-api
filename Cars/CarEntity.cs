using System;
using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Cars
{
    public class CarEntity : Entity
    {
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
        public string Model { get; set; }
        public string Company { get; set; }
        public int Year { get; set; }
        public float Autonomy { get; set; }
        public float BatteryLeft { get; set; }
        public DateTime LastTechRevision { get; set; }
    }
}