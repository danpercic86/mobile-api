using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Forum
{
    public class TopicEntity : Entity
    {    
        public string Title { get; set; }
        public string Content { get; set; }
        
        [ReadOnly(true)]
        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [ReadOnly(true)]
        public string CategoryId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(CategoryId))]
        public CategoryEntity Category { get; set; }
        [ReadOnly(true)]
        public DateTime Created { get; set; }
        [ReadOnly(true)]
        public DateTime LastEdited { get; set; }
    }
}