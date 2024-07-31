using System.ComponentModel.DataAnnotations;
namespace DatingApp.DTOs
{
    public class RegisterDto
    {

        [Required]
        public  string username { get; set; } =string.Empty;
        [Required]
        public  string KnowsAs { get; set; }=string.Empty;
        [Required]
        public string? gender { get; set; }
        [Required]
        public string? dateOfBirth { get; set; }

        [Required]
        public string? city { get; set; }

        [Required]
        public string? counry { get; set; }

        [Required]
        public  string? password { get; set; }
    }
}
