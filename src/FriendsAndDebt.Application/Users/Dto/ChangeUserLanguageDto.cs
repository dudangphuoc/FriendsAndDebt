using System.ComponentModel.DataAnnotations;

namespace FriendsAndDebt.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}