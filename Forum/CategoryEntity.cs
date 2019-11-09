using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Forum
{
    public class CategoryEntity : Entity
    {    
        
        [ReadOnly(true)]
        public string UserId { get; set; }
        
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [ReadOnly(true)]
        public string ParentCategoryId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(ParentCategoryId))]
        public  CategoryEntity ParentCategory { get; set; }
        
        public string Title { get; set; }
        [ReadOnly(true)]
        public DateTime LastEdited { get; set; }
    }
}