namespace major_data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClientEmail
    {
        public int Id { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public virtual RuleSystem RuleSystem { get; set; }
    }
}
