namespace TodoApi.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string? Message { get; set; } // Optional: for API message
    }
}
