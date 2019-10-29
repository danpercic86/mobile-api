using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace itec_mobile_api_final.Models
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}