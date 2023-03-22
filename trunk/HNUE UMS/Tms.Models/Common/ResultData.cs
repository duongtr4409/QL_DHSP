using System.Diagnostics.CodeAnalysis;

namespace Ums.Models.Common
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ResultData
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

    public static class Helper
    {
        public static ResultData ToResult(this object data, bool success = true, string message = "")
        {
            return new ResultData
            {
                data = data,
                message = message,
                success = success
            };
        }
    }
}