using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Organiza.Infrastructure.CrossCutting.Extensions
{
    /// <summary>
    /// Extensões de classe
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ClassExtensions
    {
        /// <summary>
        /// Recupera o objeto MethodInfo do método <paramref name="methodName"/> da classe <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo<T>(string methodName)
        {
            if (string.IsNullOrEmpty(methodName)) { throw new ArgumentNullException($"Parâmetro [{methodName.GetMemberName()}]: [{methodName}] inválido."); }
            return typeof(T).GetMethod(methodName) ?? throw new Exception($"Não foi possível recuperar informações do método [{methodName}] para classe [{typeof(T).Name}]");
        }

        /// <summary>
        /// Recupera o tipo de uma classe de nome <paramref name="className"/> do assembly
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Type? GetTypeByClassName<T>(string className)
            where T : class => typeof(T).Assembly.GetTypes()
                .FirstOrDefault((at) =>
                    at.IsClass
                    && at.Name.Equals($"{className}"));

        /// <summary>
        /// Recupera o nome de um membro (atributo, parâmetro, etc)
        /// COMO USAR: <paramref name="object"/>.GetMemberName()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <returns>nome do objeto informado</returns>
        public static string GetMemberName<T>(this T @object)
        {
            Expression<Func<T>> func = () => @object;
            MemberExpression expressionBody = (MemberExpression)func.Body;
            return expressionBody.Member.Name;
        } 

    }
}
