using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Forum
{
    public class TopicEntity : Entity
    {    
        
        [ReadOnly(true)]
        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [ReadOnly(true)]
        public string CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public CategoryForumEntity Category { get; set; }
        
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastEdited { get; set; }
        public string Content { get; set; }
    }
}