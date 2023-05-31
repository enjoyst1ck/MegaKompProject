using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", 
        ErrorMessage = "Hasło musi mieć co najmniej 6 znaków oraz składać się z: 1 dużej litery, 1 małej litery, 1 cyfry i 1 znaku specjalnego")]
        public string Password { get; set; }
    }
}