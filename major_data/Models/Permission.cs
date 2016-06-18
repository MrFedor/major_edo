
namespace major_data.Models
{
    using IdentityModels;
    using System.Collections.Generic;

    public class Permission
    {
        public int Id { get; set; }
        
        public virtual Secshondeportament Secshondeportament { get; set; }
        public virtual Enumpermission Enumpermission { get; set; }
        public virtual ApplicationUser AppUser { get; set; }
        //public virtual ICollection<Secshondeportament> Secshondeportaments { get; set; }
        //public virtual ICollection<Enumpermission> Enumpermissions { get; set; }
        //public virtual ICollection<ApplicationUser> AppUsers { get; set; }

    }
}
