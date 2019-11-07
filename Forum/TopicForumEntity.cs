using System;
using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Forum
{
    public class TopicForumEntity :Entity
    {    
        
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public  String Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastEdited { get; set; }
        public String Desctiption { get; set; }
        public String CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public CategoryForumEntity Category { get; set; }
        
        
    }
}