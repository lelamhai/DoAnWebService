namespace DoAnWebService.DTO.Account
{
    public class CreateAccountDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = null!;
    }
}
