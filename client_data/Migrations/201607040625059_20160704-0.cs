namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201607040 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestDeposits", "RequestStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestDeposits", "RequestStatus", c => c.Boolean());
        }
    }
}
