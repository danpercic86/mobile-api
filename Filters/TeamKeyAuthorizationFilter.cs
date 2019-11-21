using System;
using itec_mobile_api_final.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace itec_mobile_api_final.Filters
{
    public class TeamKeyAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var instanceTeamKey = EnvVarManager.Get("TEAM_KEY");

            if (string.IsNullOrEmpty(instanceTeamKey))
            {
                SetCustomResponse(context,
                    "TEAM_KEY Environment variable not set on this API instance. Please ask the organizer to set it",
                    500);
                return;
            }

            var headerTeamKey = context.HttpContext.Request.Headers["TEAM_KEY"];
            if (string.IsNullOrEmpty(headerTeamKey))
            {
                SetCustomResponse(context,
                    "TEAM_KEY header not provided. This key is specific to your team and you should receive it from the organizers.",
                    401);
                return;
            }

            if (headerTeamKey != instanceTeamKey)
            {
                SetCustomResponse(context,
                    "Invalid TEAM_KEY header value. The provided TEAM_KEY header value differs from the TEAM_KEY set on this instance.",
                    401);
            }
        }

        private void SetCustomResponse(AuthorizationFilterContext context, string message, int statusCodes)
        {
            var dr = new ObjectResult(message)
            {
                StatusCode = statusCodes
            };
            context.Result = dr;
        }
    }
}