using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Forum
{
    public class CategoryEntity : Entity
    {    
        public string Title { get; set; }
        
        [ReadOnly(true)]
        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [ReadOnly(true)]
        public string ParentId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(ParentId))]
        public CategoryEntity Parent { get; set; }
        [ReadOnly(true)]
        public DateTime LastEdited { get; set; }
    }
}