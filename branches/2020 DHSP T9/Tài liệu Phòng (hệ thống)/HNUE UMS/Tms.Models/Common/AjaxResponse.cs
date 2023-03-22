namespace Ums.Models.Common
{
    public class AjaxResponse
    {
        public static AjaxResponse New(int result = 1, string code = "", string message = "", object data = null) => new AjaxResponse(result, code, message, data);
        public AjaxResponse(int result = 1, string code = "", string message = "", object data = null)
        {
            Result = result;
            Message = message;
            Code = code;
            Data = data;
        }
        public int Result { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public object Data { get; set; }
    }
}