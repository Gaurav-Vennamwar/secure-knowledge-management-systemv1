namespace SecureKnowledgeManagementSystemv1.API.Models.Wrappers
{
    public class ApiResponse<T>
    {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public T? Data { get; set; }
            public int StatusCode { get; set; }

            // static methods to create responses quickly
            public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int statusCode = 200)
            {
                return new ApiResponse<T>
                {
                    Success = true,
                    Message = message,
                    Data = data,
                    StatusCode = statusCode
                };
            }

            public static ApiResponse<T> FailureResponse(string message, int statusCode = 400)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = message,
                    Data = default,
                    StatusCode = statusCode
                };
            }
        }
    }


