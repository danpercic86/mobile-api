using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace itec_mobile_api_final.Filters
{
    public class TeamKeyHeaderOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Security == null)
            {
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
            }
            operation.Security.Add(TeamKeySecurity);
        }

        private static readonly Dictionary<string, IEnumerable<string>> TeamKeySecurity =
            new Dictionary<string, IEnumerable<string>> {{"TEAM_KEY", new string[0]}};
    }
}