using System.ComponentModel.DataAnnotations;

namespace api
{
    public class UserModel
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(100)]
        public string Skillsets { get; set; }
        [Required]
        [StringLength(100)]
        public string Hobby { get; set; }
    }
}
