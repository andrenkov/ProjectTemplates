namespace WebCactusAPI.Models
{
    public interface ITokenService
    {
        public string BuildToken(string key, string issuer, UserDTO user);
        public bool IsTokenValid(string key, string issuer, string audience, string token);

    }
}
