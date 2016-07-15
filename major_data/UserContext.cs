
namespace major_data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Models;
    using IdentityModels;

    public class ProcedureContext : DbContext
    {
        public ProcedureContext()
            : base("DBConnection_procedur")
        {
            Database.SetInitializer<ProcedureContext>(null);// Remove default initializer
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

    }

    public class UserContext : IdentityDbContext<ApplicationUser>
    {
        public UserContext()
            : base("DBConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<UserContext>(null);// Remove default initializer
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static UserContext Create()
        {
            return new UserContext();
        }

        public UserContext(string NameConnectionStrings)
            : base(NameConnectionStrings)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            modelBuilder.Entity<RuleSystem>().HasRequired(p => p.Dogovor);
            modelBuilder.Entity<RuleSystem>().HasRequired(p => p.Department);
            modelBuilder.Entity<RuleSystem>().HasRequired(p => p.Secshondeportament);
            modelBuilder.Entity<RuleSystem>().HasRequired(p => p.AssetType);
            modelBuilder.Entity<RuleSystem>().HasOptional(p => p.Fond);
            modelBuilder.Entity<FileInSystem>().HasRequired(p => p.RuleSystem);
            modelBuilder.Entity<FileInSystem>().HasOptional(p => p.FileIn);
            modelBuilder.Entity<CBInfo>().HasOptional(p => p.TypeXML);
            modelBuilder.Entity<Dogovor>().HasRequired(p => p.Client);
            modelBuilder.Entity<ApplicationUser>().HasOptional(p => p.Department);
            modelBuilder.Entity<ApplicationUser>().HasOptional(p => p.Secshondeportament);

            modelBuilder.Entity<CBInfo>()
                .HasRequired(t => t.FileInSystem)
                .WithOptional(t => t.CBInfo)
                .Map(m => m.MapKey("FileInSystemId"))
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<FileRequst>()
                .HasRequired(t => t.FileInSystem)
                .WithOptional(t => t.FileRequst)
                .Map(m => m.MapKey("FileInSystemId"))
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CBCert>()
                .HasRequired(t => t.CBInfo)
                .WithMany(t => t.CBCerts)
                .Map(m => m.MapKey("CBInfoID"))
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RuleUser>()
                .HasRequired(t => t.RuleSystem)
                .WithMany(t => t.RuleUsers)
                .Map(m => m.MapKey("RuleSystem_Id"))
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RuleUser>()
                .HasRequired(t => t.AppUser)
                .WithMany(t => t.RuleUsers)
                .Map(m => m.MapKey("AppUser_Id"))
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Certificate>()
                .HasRequired(t => t.AppUser)
                .WithMany(t => t.Certificates)
                .Map(m => m.MapKey("AppUser_Id"))
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Permission>()
                .HasRequired(t => t.AppUser)
                .WithMany(t => t.Permissions)
                .Map(m => m.MapKey("AppUser_Id"))
                .WillCascadeOnDelete(true);
        }

        public DbSet<FileInSystem> FileInSystem { get; set; }
        public DbSet<CBCert> CBCert { get; set; }
        public DbSet<CBInfo> CBInfo { get; set; }
        public DbSet<AssetType> AssetType { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Fond> Fond { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ClientEmail> ClientEmail { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<Dogovor> Dogovor { get; set; }
        public DbSet<RuleSystem> RuleSystem { get; set; }
        public DbSet<TypeXML> TypeXML { get; set; }
        public DbSet<RuleUser> RuleUser { get; set; }
        public DbSet<Secshondeportament> Secshondeportament { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Enumpermission> Enumpermission { get; set; }
        public DbSet<FileRequst> FileRequst { get; set; }
        public DbSet<SettingsDirectory> SettingsDirectory { get; set; }
    }
}
