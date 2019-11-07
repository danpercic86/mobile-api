using System;
using System.ComponentModel.DataAnnotations.Schema;
using itec_mobile_api_final.Entities;
using Newtonsoft.Json;

namespace itec_mobile_api_final.Forum
{
    public class MessageForumEntity :Entity
    {    
        
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastEdited { get; set; }
        public  String TopicId { get; set; }
        [ForeignKey(nameof(TopicId))]
        public TopicForumEntity Topic { get; set; }
    }
}