using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using itec_mobile_api_final.Base;

namespace itec_mobile_api_final.Entities
{
    public class Entity
    {
        [Key]
        [ReadOnly(true)] public string Id { get; set; }

        [ReadOnly(true)] public DateTime Created { get; set; }

        [ReadOnly(true)] public DateTime Updated { get; set; }
    }
}