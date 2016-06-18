namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606163 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RuleSystem", "Secshondeportament_Id", "dbo.Secshondeportament");
            DropIndex("dbo.RuleSystem", new[] { "Secshondeportament_Id" });
            AddColumn("dbo.Enumpermission", "Description", c => c.String());
            AlterColumn("dbo.RuleSystem", "Secshondeportament_Id", c => c.Int());
            CreateIndex("dbo.RuleSystem", "Secshondeportament_Id");
            AddForeignKey("dbo.RuleSystem", "Secshondeportament_Id", "dbo.Secshondeportament", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RuleSystem", "Secshondeportament_Id", "dbo.Secshondeportament");
            DropIndex("dbo.RuleSystem", new[] { "Secshondeportament_Id" });
            AlterColumn("dbo.RuleSystem", "Secshondeportament_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Enumpermission", "Description");
            CreateIndex("dbo.RuleSystem", "Secshondeportament_Id");
            AddForeignKey("dbo.RuleSystem", "Secshondeportament_Id", "dbo.Secshondeportament", "Id", cascadeDelete: true);
        }
    }
}
