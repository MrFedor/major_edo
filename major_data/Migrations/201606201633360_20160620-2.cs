namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606202 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.RuleSystem", name: "AssetType_Id", newName: "AssetTypeId");
            RenameColumn(table: "dbo.RuleSystem", name: "Department_Id", newName: "DepartmentId");
            RenameColumn(table: "dbo.RuleSystem", name: "Dogovor_Id", newName: "DogovorId");
            RenameColumn(table: "dbo.RuleSystem", name: "Fond_Id", newName: "FondId");
            RenameColumn(table: "dbo.RuleSystem", name: "Secshondeportament_Id", newName: "SecshondeportamentId");
            RenameIndex(table: "dbo.RuleSystem", name: "IX_Dogovor_Id", newName: "IX_DogovorId");
            RenameIndex(table: "dbo.RuleSystem", name: "IX_Department_Id", newName: "IX_DepartmentId");
            RenameIndex(table: "dbo.RuleSystem", name: "IX_Secshondeportament_Id", newName: "IX_SecshondeportamentId");
            RenameIndex(table: "dbo.RuleSystem", name: "IX_AssetType_Id", newName: "IX_AssetTypeId");
            RenameIndex(table: "dbo.RuleSystem", name: "IX_Fond_Id", newName: "IX_FondId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.RuleSystem", name: "IX_FondId", newName: "IX_Fond_Id");
            RenameIndex(table: "dbo.RuleSystem", name: "IX_AssetTypeId", newName: "IX_AssetType_Id");
            RenameIndex(table: "dbo.RuleSystem", name: "IX_SecshondeportamentId", newName: "IX_Secshondeportament_Id");
            RenameIndex(table: "dbo.RuleSystem", name: "IX_DepartmentId", newName: "IX_Department_Id");
            RenameIndex(table: "dbo.RuleSystem", name: "IX_DogovorId", newName: "IX_Dogovor_Id");
            RenameColumn(table: "dbo.RuleSystem", name: "SecshondeportamentId", newName: "Secshondeportament_Id");
            RenameColumn(table: "dbo.RuleSystem", name: "FondId", newName: "Fond_Id");
            RenameColumn(table: "dbo.RuleSystem", name: "DogovorId", newName: "Dogovor_Id");
            RenameColumn(table: "dbo.RuleSystem", name: "DepartmentId", newName: "Department_Id");
            RenameColumn(table: "dbo.RuleSystem", name: "AssetTypeId", newName: "AssetType_Id");
        }
    }
}
