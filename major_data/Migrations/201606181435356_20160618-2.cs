namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606182 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Permission", "Secshondeportament_Id", "dbo.Secshondeportament");
            DropIndex("dbo.Permission", new[] { "Secshondeportament_Id" });
            AddColumn("dbo.Permission", "IdSecshondeportament", c => c.Int(nullable: false));
            DropColumn("dbo.Permission", "Secshondeportament_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Permission", "Secshondeportament_Id", c => c.Int());
            DropColumn("dbo.Permission", "IdSecshondeportament");
            CreateIndex("dbo.Permission", "Secshondeportament_Id");
            AddForeignKey("dbo.Permission", "Secshondeportament_Id", "dbo.Secshondeportament", "Id");
        }
    }
}
