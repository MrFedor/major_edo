namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606301 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileRequst", "FileStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.FileRequst", "FileType", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileRequst", "FileType");
            DropColumn("dbo.FileRequst", "FileStatus");
        }
    }
}
