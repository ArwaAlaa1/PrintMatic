using System;

namespace PrintMatic.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "خطأ فالطلب",
                401 => "اسم المستخدم أو كلمة المرور غير صحيحة",
                404 => "غير موجود",
                500 => "Errors are the path to the dark side. Errors lead to anger.  Anger leads to hate.  Hate leads to career change",
                _ => null 
            };
        }
    }
}
