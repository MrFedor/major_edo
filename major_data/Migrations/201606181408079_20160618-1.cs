namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606181 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Permission", "Enumpermission_Id", "dbo.Enumpermission");
            DropIndex("dbo.Permission", new[] { "Enumpermission_Id" });
            DropPrimaryKey("dbo.Enumpermission");
            AddColumn("dbo.Permission", "EnumpermissionName", c => c.String());
            AlterColumn("dbo.Enumpermission", "Name", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Enumpermission", "Name");
            DropColumn("dbo.Permission", "Enumpermission_Id");
            DropColumn("dbo.Enumpermission", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Enumpermission", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Permission", "Enumpermission_Id", c => c.Int());
            DropPrimaryKey("dbo.Enumpermission");
            AlterColumn("dbo.Enumpermission", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Permission", "EnumpermissionName");
            AddPrimaryKey("dbo.Enumpermission", "Id");
            CreateIndex("dbo.Permission", "Enumpermission_Id");
            AddForeignKey("dbo.Permission", "Enumpermission_Id", "dbo.Enumpermission", "Id");
        }
    }
}
