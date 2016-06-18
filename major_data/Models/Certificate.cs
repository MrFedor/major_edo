namespace major_data.Models
{
    using IdentityModels;
    using System.ComponentModel.DataAnnotations;

    public class Certificate
    {
        public int Id { get; set; }

        [Display(Name = "Серийный номер сертификата")]
        public string SerialNumber { get; set; }

        [Display(Name = "Активный сертификат")]
        public bool IsActive { get; set; }

        public virtual ApplicationUser AppUser { get; set; }
    }
}
