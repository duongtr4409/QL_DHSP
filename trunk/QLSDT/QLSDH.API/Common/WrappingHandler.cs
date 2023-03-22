using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace QLSDH.API.Common
{
    public class WrappingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            return BuildApiResponse(request, response);
        }

        private static HttpResponseMessage BuildApiResponse(System.Net.Http.HttpRequestMessage request, HttpResponseMessage response)
        {
            object content;
            string errorMessage = null;

            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                HttpError error = content as HttpError;

                if (error != null)
                {
                    content = null;
                    errorMessage = error.Message;

#if DEBUG
                    errorMessage = string.Concat(errorMessage, error.ExceptionMessage, error.StackTrace);
#endif
                }
            }

            var newResponse = request.CreateResponse(response.StatusCode, new ApiResponse(response.StatusCode, content, errorMessage));

            foreach (var header in response.Headers)
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }

            return newResponse;
        }
    }
    /// <summary>
    ///  ApiResponse
    /// </summary>
    [DataContract]
    public class ApiResponse
    {
        /// <summary>
        ///  Version
        /// </summary>
        [DataMember]
        public string Version { get { return "1.0"; } }

        /// <summary>
        /// StatusCode
        /// </summary>
        [DataMember]
        public int Code { get; set; }
        /// <summary>
        ///   ErrorMessage
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; }

        /// <summary>
        /// Result 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public object Result { get; set; }

        /// <summary>
        /// ApiResponse
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="result"></param>
        /// <param name="errorMessage"></param>
        public ApiResponse(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            Code = (int)statusCode;
            Result = result;
            Message = errorMessage;
        }
        public ApiResponse(int statusCode, object result = null, string errorMessage = null)
        {
            Code = statusCode;
            Result = result;
            Message = errorMessage;
        }
    }
    public class ApiResponseCode
    {
        public const int SUCCESS_CODE = 200;
        public const int LOGGIC_CODE = -1;
        public const int VALIDATOR_CODE = -2; // Lỗi nhập dữ liệu
        public const int ACCOUNT_EXIST_CODE = -22; // Lỗi tai khoan da tom tai
        public const int EXCEPTION_CODE = -3;   // Lỗi khi có exception
        public const int INVITENUMBER_CODE = -4; // Số dt giới thiệu không đúng
        public const int INVALIDE_PHONE_NUMBER_CODE = -5; // Số dt không hợp lệ
        public const int INVITE_USER_NOTFOUND_CODE = -6; // Người giới thiệu không tồn tại
        public const int USER_NOTFOUND = -7; // Không tìm thấy tài khoản
        public const int ACTIVE_ERROR = -8; // Lỗi không thể kích hoạt tài khoản
        public const int OTP_INCORRECT = -9; // OTP không đúng
        public const int REGISTER_FAILURE = -10;// Lỗi không thể tạo user
        public const int REGISTER_LOCK_FAILURE = -11;// Lỗi không thể lock user
        public const int SENDSMS_FAILURE = -12;// Lỗi không thể lock user
        public const int LOGIN_ERROR = -13;// Lỗi không thể đăng nhập

        public const int UPLOAD_READ_IMAGE = -14;// Lỗi khi đọc file
        public const int UPLOAD_FILE_INVALID = -15;// CHưa chọn file
        public const int UPLOAD_FILE_SIZE_INVALID = -16;// CHưa chọn file
        public const int UPLOAD_FILE_NOT_SELECTED = -17;// CHưa chọn file

        public const int BUY_CARD_FAILURE = -18;// Lỗi không thể mua dc the
        public const int TOPUP_CARD_FAILURE = -19;// Lỗi không thể mua dc the
        public const int TRANSFER_CARD_FAILURE = -20;// Lỗi không thể chuyen dc tien
        public const int SEND_NOTIFICATION_ERRORR = -21; // Lỗi không thể kích hoạt tài khoản

        public const int PRICE_INVALID = -22; // Gia lon hon 0
        public const int SERVICE_CODE_INVALID = -23; // Ma giao dich k dung
        public const int CHANGE_PASS_WORD_FAILURE = -24; // Ma giao dich k dung
        public const int FORGOT_PASSWORD_FAILURE = -24; // Ma giao dich k dung
        public const int BALANCE_NOT_VALID = -25;// So du khong đủ
        public const int VAN_DON_DA_DUOCNHAN = -26;// So du khong đủ

        public const int CARD_VALID = -27;// So du khong đủ
        public const int PRICEINPUT_INVALID = -28; //Số tiền nhập vào không đúng

    }
}