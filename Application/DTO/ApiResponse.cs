using System.Text.Json.Serialization;

namespace Application.DTO
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }

        public string Status => (StatusCode >= 200 && StatusCode <= 299) ? "success" : "error";

        public string Message { get; set; } = string.Empty;

        // IgnoreCondition asegura que si Data es null (ej. en un error), no aparezca en el JSON final
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data, string message = "Operación exitosa", int statusCode = 200)
        {
            return new ApiResponse<T> { StatusCode = statusCode, Message = message, Data = data };
        }

        public static ApiResponse<T> Error(string message, int statusCode = 400)
        {
            return new ApiResponse<T> { StatusCode = statusCode, Message = message };
        }
    }
}
