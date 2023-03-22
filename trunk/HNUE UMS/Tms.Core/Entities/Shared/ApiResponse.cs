namespace Ums.Core.Entities.Shared
{
    public class ApiResponse
    {
        public bool success { get; set; }
        public object data { get; set; }
        public int total { get; set; }
        public string message { get; set; }
    }
    public static class ApiExtensions
    {
        public static ApiResponse CreateResponse(this object _data, bool _success = true, string _message = "", int total = 0)
        {
            return new ApiResponse { success = _success, data = _success ? _data : null, message = _message, total = total };
        }
    }
}