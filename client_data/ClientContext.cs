
namespace client_data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using IdentityModels;
    using Models;    
    public class ClientContext : IdentityDbContext<ApplicationUser>
    {
        public ClientContext()
            : base("DBConnection_client", throwIfV1Schema: false)
        {
            Database.SetInitializer<ClientContext>(null);// Remove default initializer
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ClientContext Create()
        {
            return new ClientContext();
        }

        public ClientContext(string NameConnectionStrings)
            : base(NameConnectionStrings)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<RequestDeposits> RequestDeposits { get; set; }
        public DbSet<DocumentCollections> DocumentCollections { get; set; }
        public DbSet<PercentPeriods> PercentPeriods { get; set; }
        public DbSet<DogovorDeposits> DogovorDeposits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            modelBuilder.Entity<RequestDeposits>().Property(p => p.DepositSum).HasColumnType("decimal").HasPrecision(22, 7);
            modelBuilder.Entity<RequestDeposits>().Property(p => p.BalanceMin).HasColumnType("decimal").HasPrecision(22, 7);
            modelBuilder.Entity<RequestDeposits>().Property(p => p.RateValue).HasColumnType("decimal").HasPrecision(22, 7);
            modelBuilder.Entity<RequestDeposits>().Property(p => p.RateValueNercent).HasColumnType("decimal").HasPrecision(22, 7);
        }
        

    }    
}
