namespace WorkMateBE.Responses
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public ApiResponse()
        {
            StatusCode = 200; // Gán giá trị mặc định
            Message = string.Empty; // Chuỗi trống để tránh null
        }
    }
}
