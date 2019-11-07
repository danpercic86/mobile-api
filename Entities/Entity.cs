using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using itec_mobile_api_final.Base;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Entities
{
    public class Entity
    {
        [Key]
        [ReadOnly(true)] public string Id { get; set; }
        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}