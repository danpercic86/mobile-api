using Microsoft.AspNetCore.Identity;

namespace itec_mobile_api_final.Data.User
{
    public class UserIdentity : IdentityUser
    {
        public override string Id { get; set; }
    }
}