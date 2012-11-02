using System.ComponentModel.DataAnnotations;

namespace putaty.web.Areas.SuperUser.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Юзернейм")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Кодовое слово")]
        public string Pazz { get; set; }
    }
}