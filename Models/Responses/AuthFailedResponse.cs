using System.Collections.Generic;

namespace itec_mobile_api_final.Models.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}