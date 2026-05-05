namespace DoAnWebService.DTO.Account
{
    public class AccountDTO
    {
        public int AccountId { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;

        public bool Active { get; set; }
    }
}
