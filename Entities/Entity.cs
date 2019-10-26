using System;
using itec_mobile_api_final.Base;

namespace itec_mobile_api_final.Entities
{
    public class Entity: IEntity
    {
        [IsReadOnly] public string Id { get; set; }

        [IsReadOnly] public DateTime Created { get; set; }

        [IsReadOnly] public DateTime Updated { get; set; }
    }
}