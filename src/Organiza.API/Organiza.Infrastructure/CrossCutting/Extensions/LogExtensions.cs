using Newtonsoft.Json;
using Organiza.Infrastructure.CrossCutting.Extensions.JsonConverters;
using System.Diagnostics.CodeAnalysis;

namespace Organiza.Infrastructure.CrossCutting.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class LogExtensions
    {
        public static string MontarLog<T>(string nome, T objeto)
        {
            if (objeto != null)
                return $"{nome}: {JsonConvert.SerializeObject(objeto, new AnonymizeJsonConverter())}";

            return $"O objeto {nome} é nulo.";
        }

        public static string MontarLog<T>(string nome, Exception ex, T objeto)
        {
            if (objeto != null)
                return $"{nome}: Exception: {ex.CompleteExceptionWithStackTrace()}{Environment.NewLine}{JsonConvert.SerializeObject(objeto, new AnonymizeJsonConverter())}";

            return $"O objeto {nome} é nulo.";
        }
    }
}
