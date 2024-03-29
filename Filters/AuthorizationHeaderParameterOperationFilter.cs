using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace itec_mobile_api_final.Filters
{
    public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter)
                .Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter)
                .Any(filter => filter is IAllowAnonymousFilter);

            if (!isAuthorized || allowAnonymous) return;

            if (operation.Security == null)
            {
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
            }
            operation.Security.Add(BearerSecurity);
        }

        private static readonly Dictionary<string, IEnumerable<string>> BearerSecurity =
            new Dictionary<string, IEnumerable<string>> {{"Bearer", new string[0]}};
    }
}