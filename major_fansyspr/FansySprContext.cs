
namespace major_fansyspr
{
    using Models;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    public class FansySprContext : DbContext
    {
        public FansySprContext()
            : base("FansySprConnection")
        {
            Database.SetInitializer<FansySprContext>(null);
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Client> Client { get; set; }
        public DbSet<Dogovor> Dogovor { get; set; }
        public DbSet<Banks> Banks { get; set; }
        public DbSet<BanksAccount> BanksAccount { get; set; }
        public DbSet<OD_USR_TABS> OD_USR_TABS { get; set; }
    }
}
