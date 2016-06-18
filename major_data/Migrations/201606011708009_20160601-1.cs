namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606011 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Department_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Department_Id");
            AddForeignKey("dbo.AspNetUsers", "Department_Id", "dbo.Department", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Department_Id", "dbo.Department");
            DropIndex("dbo.AspNetUsers", new[] { "Department_Id" });
            DropColumn("dbo.AspNetUsers", "Department_Id");
        }
    }
}
