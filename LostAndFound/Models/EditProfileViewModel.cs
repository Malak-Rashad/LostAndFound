using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Models
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;
        [Display(Name = "إظهار رقم الموبايل للآخرين")]
        public bool ShowPhone { get; set; }

        [Display(Name = "إظهار الإيميل للآخرين")]
        public bool ShowEmail { get; set; }

        [Display(Name = "رقم الموبايل")]
        [Phone(ErrorMessage = "رقم الموبايل غير صحيح")]
        public string Phone { get; set; } = string.Empty;
        [Display(Name = "المحافظة")]
        public LostAndFound.Enums.City? City { get; set; }

        [Display(Name = "الصورة الشخصية")]
        public IFormFile? AvatarFile { get; set; }

        public string? CurrentAvatarBase64 { get; set; }
    }
}