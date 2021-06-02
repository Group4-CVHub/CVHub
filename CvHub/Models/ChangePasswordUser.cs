using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CVHub.Models
{
    public class ChangePasswordUser
    {
        public int UserId { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required, PasswordPropertyText, StringLength(150, MinimumLength = 5), DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
