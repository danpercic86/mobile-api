using System;
using itec_mobile_api_final.Entities;
using Microsoft.AspNetCore.Identity;

namespace itec_mobile_api_final.Cars
{
    public class CarEntity : Entity
    {
        public IdentityUser User { get; set; }
        public string Model { get; set; }
        public string Company { get; set; }
        public DateTime Year { get; set; }
        public float Autonomy { get; set; }
        public float BatteryLeft { get; set; }
        public DateTime LastTechRevision { get; set; }
    }
}