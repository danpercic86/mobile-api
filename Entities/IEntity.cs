using System;
using itec_mobile_api_final.Base;


namespace itec_mobile_api_final.Entities
{
    public interface IEntity
    {
        [IsReadOnly] string Id { get; set; }

        [IsReadOnly] DateTime Created { get; set; }

        [IsReadOnly] DateTime Updated { get; set; }
    }
}