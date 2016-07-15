namespace major_data.Migrations
{ 
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606180 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permission", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Permission", "IsActive");
        }
    }
}
