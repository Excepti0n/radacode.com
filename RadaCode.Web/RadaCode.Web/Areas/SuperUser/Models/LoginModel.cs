using System.ComponentModel.DataAnnotations;

namespace RadaCode.Web.Areas.SuperUser.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Pazz { get; set; }
    }
}