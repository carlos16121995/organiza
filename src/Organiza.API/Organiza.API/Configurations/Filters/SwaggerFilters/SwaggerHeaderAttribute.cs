// Copyright (c) 2022, Unidas. All rights reserved
// PRIVATE SOURCE. Any kind of unauthorized use is prohibited.

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Diagnostics.CodeAnalysis;

namespace Organiza.API.Configurations.Filters.SwaggerFilters
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerHeaderAttribute : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context) => operation.Parameters ??= new List<OpenApiParameter>();
    }
}
