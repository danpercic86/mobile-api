using System;
using System.ComponentModel.DataAnnotations;
using itec_mobile_api_final.Base;

namespace itec_mobile_api_final.Entities
{
    public class Entity: IEntity
    {
        [Key]
        [IsReadOnly] public string Id { get; set; }

        [IsReadOnly] public DateTime Created { get; set; }

        [IsReadOnly] public DateTime Updated { get; set; }
    }
}