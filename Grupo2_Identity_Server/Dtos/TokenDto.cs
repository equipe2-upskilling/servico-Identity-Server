namespace Grupo2_Identity_Server.Dtos
{
    public class TokenDto
    {
        public bool Authenticated { get; set; }
        public DateTime Expiration { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
    }
}
