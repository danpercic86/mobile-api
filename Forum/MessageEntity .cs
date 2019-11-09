using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Forum
{
    public class MessageEntity : Entity
    {    
        
        [ReadOnly(true)]
        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [ReadOnly(true)]
        public  string TopicId { get; set; }
        [ForeignKey(nameof(TopicId))]
        public TopicEntity Topic { get; set; }
        
        public string Message { get; set; }
        [ReadOnly(true)]
        public DateTime Created { get; set; }
        [ReadOnly(true)]
        public DateTime LastEdited { get; set; }
    }
}