namespace Pre.UserProjectManager.Core.Models.APIResponse
{
    public class GenericResponse<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public GenericResponse(int statusCode, bool success, string msg, T data)
        {
            Data = data;
            Succeeded = success;
            StatusCode = statusCode;
            Message = msg;
        }
        public GenericResponse()
        {
        }

        public static GenericResponse<T> Fail(string errorMessage, int statusCode = 400)
        {
            return new GenericResponse<T> { Succeeded = false, Message = errorMessage, StatusCode = statusCode };
        }
        public static GenericResponse<T> Error(string errorMessage, int statusCode = 500)
        {
            return new GenericResponse<T> { Succeeded = false, Message = errorMessage, StatusCode = statusCode };
        }
        public static GenericResponse<T> UnAuthorized(string errorMessage, int statusCode = 401)
        {
            return new GenericResponse<T> { Succeeded = false, Message = errorMessage, StatusCode = statusCode };
        }
        public static GenericResponse<T> Success(string successMessage, T data, int statusCode = 200)
        {
            return new GenericResponse<T> { Succeeded = true, Message = successMessage, Data = data, StatusCode = statusCode };
        }
        public static GenericResponse<T> Success(string successMessage, int statusCode = 200)
        {
            return new GenericResponse<T> { Succeeded = true, Message = successMessage, StatusCode = statusCode };
        }
        public static GenericResponse<T> Created(string successMessage, T data, int statusCode = 201)
        {
            return new GenericResponse<T> { Succeeded = true, Message = successMessage, Data = data, StatusCode = statusCode };
        }
        public static GenericResponse<T> NotFound(string successMessage, int statusCode = 404)
        {
            return new GenericResponse<T> { Succeeded = false, Message = successMessage, StatusCode = statusCode };
        }
    }
}
