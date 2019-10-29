using System.ComponentModel.DataAnnotations.Schema;

namespace itec_mobile_api_final.Models.Requests
{
    public class UserRegistrationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}