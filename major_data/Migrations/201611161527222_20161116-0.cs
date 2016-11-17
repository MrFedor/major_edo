namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201611160 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileInSystem", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileInSystem", "Comment");
        }
    }
}
