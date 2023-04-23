using System.Text;

namespace Organiza.Infrastructure.CrossCutting.Extensions
{
    public static class ExceptionExtensions
    {
        public static string FullMessage(this Exception ex)
        {
            try
            {
                if (ex is AggregateException aex)
                    return aex.InnerExceptions.Aggregate("", (total, next) => $"{total}[{next.FullMessage()}] ");
                var msg = ex.Message.Replace(", see inner exception.", "").Trim();
                var innerMsg = ex.InnerException?.FullMessage();
                if (innerMsg is object && innerMsg != msg)
                    msg = $"{msg} [ {innerMsg} ]";
                return msg;
            }
            catch
            {
                return "error get full message!";
            }
        }

        public static string CompleteExceptionWithStackTrace(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            var _ex = ex;

            while (_ex != null)
            {
                sb.AppendLine(_ex.Message);
                _ex = _ex.InnerException;
            }

            sb.AppendLine(ex?.StackTrace);

            return sb.ToString();
        }
    }
}
