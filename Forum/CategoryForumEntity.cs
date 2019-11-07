using System;
using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Forum
{
    public class CategoryForumEntity :Entity
    {    
        
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public DateTime LastEdited { get; set; }
        public DateTime LastTechRevision { get; set; }
        public string Title { get; set; }
        public string CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public  CategoryForumEntity ParentCategory { get; set; }
    }
}