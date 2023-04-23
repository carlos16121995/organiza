// Copyright (c) 2022, Unidas. All rights reserved
// PRIVATE SOURCE. Any kind of unauthorized use is prohibited.

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Organiza.API.Configurations.Filters.SwaggerFilters
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerEnumDescriptions : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var property in swaggerDoc.Components.Schemas.Where(x => x.Value?.Enum?.Count > 0))
            {
                var bld = new StringBuilder();
                var propertyEnums = property.Value.Enum;
                if (propertyEnums != null && propertyEnums.Count > 0)
                    bld.Append(DescribeEnum(propertyEnums, property.Key));
                property.Value.Description = bld.ToString();
            }

            foreach (var pathItem in swaggerDoc.Paths.Values)
                DescribeEnumParameters(pathItem.Operations, swaggerDoc);
        }

        private static void DescribeEnumParameters(IDictionary<OperationType, OpenApiOperation> operations, OpenApiDocument swaggerDoc)
        {

            if (operations != null)
                foreach (var oper in operations)
                    foreach (var param in oper.Value.Parameters)
                    {
                        var bld = new StringBuilder();
                        var paramEnum = swaggerDoc.Components.Schemas.FirstOrDefault(x => x.Key == ToPascalCase(param.Name));
                        if (paramEnum.Value != null)
                        {
                            bld.Append(DescribeEnum(paramEnum.Value.Enum, paramEnum.Key));
                            param.Description = bld.ToString();
                        }
                    }
        }
        private static string ToPascalCase(string input)
        {
            string[] words = input.Split('_');
            return string.Join("", words.Select(w => char.ToUpper(w[0]) + w[1..].ToLower()));
        }

        private static Type GetEnumTypeByName(string enumTypeName) => AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .First(x => x.Name == enumTypeName);

        private static string? DescribeEnum(IList<IOpenApiAny> enums, string proprtyTypeName)
        {
            var enumDescriptions = new List<string>();
            var enumType = GetEnumTypeByName(proprtyTypeName);
            if (enumType == null)
                return null;

            enumDescriptions.AddRange(from OpenApiInteger enumOption in enums
                                      let enumInt = enumOption.Value
                                      select string.Format("{0} = {1}", enumInt, Enum.GetName(enumType, enumInt)));
            return string.Join(", ", enumDescriptions.ToArray());
        }
    }
}
