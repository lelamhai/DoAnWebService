namespace DoAnWebService.DTO.User
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpiredToken { get; set; }
    }
}
