namespace Readify.DTO.Users
{
    public class UserInfoDTO
    {
        public int Id { get; set; }

        public string Nickname { get; set; } = null!;

        public string AvatarImagePath { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
