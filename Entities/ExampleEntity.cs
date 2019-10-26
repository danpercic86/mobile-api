using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace itec_mobile_api_final.Entities
{
    [Table("Example",Schema ="dbo")]  
    public class ExampleEntity  
    {  
        [Key]  
      
        public Guid ExampleId { get; set; }
    } 
}