namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606303 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestDeposits", "ChildCat", c => c.Int(nullable: false));
            AddColumn("dbo.RequestDeposits", "Operation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestDeposits", "Operation");
            DropColumn("dbo.RequestDeposits", "ChildCat");
        }
    }
}
