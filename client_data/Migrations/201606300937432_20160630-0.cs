namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606300 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestDeposits", "RequestId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestDeposits", "RequestId");
        }
    }
}
